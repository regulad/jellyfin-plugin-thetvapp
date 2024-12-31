using System.Runtime.CompilerServices;
using System.Web;
using Jellyfin.Plugin.TheTVApp.Timers;
using MediaBrowser.Controller.Entities.TV;

namespace Jellyfin.Plugin.TheTVApp;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using MediaBrowser.Controller.LiveTv;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.LiveTv;
using Microsoft.Extensions.Logging;

/// <summary>
/// Live TV service for thetvapp.to.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Initialization is implicit by the plugin loader, no opportunity to teardown")]
public class LiveTvService : ILiveTvService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<LiveTvService> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };

    // cache
    private readonly SemaphoreSlim channelCacheLock = new SemaphoreSlim(1, 1);
    private readonly SemaphoreSlim dvrTimerModifyLock = new SemaphoreSlim(1, 1);
    private readonly TimeSpan cacheDuration = TimeSpan.FromMinutes(5);
    private readonly VideoDecryption videoDecryption;
    private IEnumerable<TheTvAppChannel>? tvAppChannelsCache;
    private DateTime lastFetchTime = DateTime.MinValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveTvService"/> class.
    /// This constructor is only fired by the plugin loader.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "SA1600", Justification = "Injected by the plugin loader")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "SA1611", Justification = "Injected by the plugin loader")]
    public LiveTvService(IHttpClientFactory httpClientFactory, ILogger<LiveTvService> logger)
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;
        this.videoDecryption = new VideoDecryption(logger);

        this.logger.LogDebug("TheTVApp LiveTvService initialized.");
    }

    /// <inheritdoc />
    public string HomePageUrl
    {
        get { return "https://thetvapp.to/"; }
    }

    /// <inheritdoc />
    public string Name
    {
        get { return "TheTVApp LiveTvService"; }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ChannelInfo>> GetChannelsAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("GetChannelsAsync called.");

        var dedupedAppChannels = await this.GetTvAppChannelsAsync(cancellationToken).ConfigureAwait(false);

        // may want to cache here in the future
        var channelInfos = dedupedAppChannels.Select(channel => channel.ToChannelInfo());

        logger.LogDebug("GetChannelsAsync returning: {0}", channelInfos);

        return channelInfos;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProgramInfo>> GetProgramsAsync(string channelId, DateTime startDateUtc, DateTime endDateUtc, CancellationToken cancellationToken)
    {
        logger.LogDebug("GetProgramsAsync called for channel {0}.", channelId);

        var dedupedAppChannels = await this.GetTvAppChannelsAsync(cancellationToken).ConfigureAwait(false);
        var channel = dedupedAppChannels.FirstOrDefault(channel => channel.Callsign == channelId);

        if (channel == null)
        {
            return new List<ProgramInfo>();
        }

        // filtering this is unnecessary because guide entries out crazy far aren't even returned in the guide
        var programs = channel.ToProgramInfos();

        logger.LogDebug("GetProgramsAsync returning: {0}", programs);

        return programs;
    }

    /// <inheritdoc />
    public async Task<MediaSourceInfo> GetChannelStream(string channelId, string streamId, CancellationToken cancellationToken)
    {
        logger.LogDebug("GetChannelStream called.");

        var mediaSourceStreams = await this.GetChannelStreamMediaSources(channelId, cancellationToken).ConfigureAwait(false);
        var mediaSource = mediaSourceStreams.FirstOrDefault(mediaSource => mediaSource.Id == streamId);

        if (mediaSource == null)
        {
            logger.LogWarning("Stream {0} for channel {1} not found.", streamId, channelId);
            throw new ArgumentException("Stream not found.");
        }

        logger.LogDebug("GetChannelStream returning: {0}", mediaSource);

        return mediaSource;
    }

    /// <inheritdoc />
    public async Task<List<MediaSourceInfo>> GetChannelStreamMediaSources(string channelId, CancellationToken cancellationToken)
    {
        logger.LogDebug("GetChannelStreamMediaSources called for channel ID {0}.", channelId);

        using (var httpClient = this.httpClientFactory.CreateClient())
        {
            var dedupedAppChannels = await this.GetTvAppChannelsAsync(cancellationToken).ConfigureAwait(false);
            var channel = dedupedAppChannels.FirstOrDefault(channel => channel.ToChannelInfo().Id == channelId);

            if (channel == null)
            {
                return new List<MediaSourceInfo>();
            }

            var mediaSources = await channel.ToMediaSourceInfosAsync(this.logger, httpClient).ConfigureAwait(false);

            var mediaSourcesList = mediaSources.ToList();

            logger.LogDebug("GetChannelStreamMediaSources returning: {0}", mediaSourcesList);

            return mediaSourcesList;
        }
    }

    /// <inheritdoc />
    // DVR methods follow
    public async Task CancelTimerAsync(string timerId, CancellationToken cancellationToken)
    {
        logger.LogDebug("CancelTimerAsync called.");

        await this.dvrTimerModifyLock.WaitAsync(cancellationToken).ConfigureAwait(true);
        try
        {
            var timerInfos = Plugin.Instance!.Configuration.TimerInfos;
            var maybeTarget = timerInfos.FirstOrDefault(tI => tI.Id == timerId);
            if (maybeTarget != null)
            {
                var mutableTimerInfos = timerInfos.ToList();
                mutableTimerInfos.Remove(maybeTarget);
                Plugin.Instance!.Configuration.TimerInfos = mutableTimerInfos.ToImmutableArray();
            }
        }
        finally
        {
            this.dvrTimerModifyLock.Release();
        }
    }

    /// <inheritdoc />
    public async Task CancelSeriesTimerAsync(string timerId, CancellationToken cancellationToken)
    {
        logger.LogDebug("CancelSeriesTimerAsync called.");

        await this.dvrTimerModifyLock.WaitAsync(cancellationToken).ConfigureAwait(true);
        try
        {
            var timerInfos = Plugin.Instance!.Configuration.SeriesTimerInfos;
            var maybeTarget = timerInfos.FirstOrDefault(tI => tI.Id == timerId);
            if (maybeTarget != null)
            {
                var mutableTimerInfos = timerInfos.ToList();
                mutableTimerInfos.Remove(maybeTarget);
                Plugin.Instance!.Configuration.SeriesTimerInfos = mutableTimerInfos.ToImmutableArray();
            }
        }
        finally
        {
            this.dvrTimerModifyLock.Release();
        }
    }

    /// <inheritdoc />
    public async Task CreateTimerAsync(TimerInfo info, CancellationToken cancellationToken)
    {
        logger.LogDebug("CreateTimerAsync called.");

        await this.dvrTimerModifyLock.WaitAsync(cancellationToken).ConfigureAwait(true);
        try
        {
            var timerInfos = Plugin.Instance!.Configuration.TimerInfos;
            var mutableTimerInfos = timerInfos.ToList();
            mutableTimerInfos.Add(new SerializableTimerInfo(info));
            Plugin.Instance!.Configuration.TimerInfos = mutableTimerInfos.ToImmutableArray();
        }
        finally
        {
            this.dvrTimerModifyLock.Release();
        }
    }

    /// <inheritdoc />
    public async Task CreateSeriesTimerAsync(SeriesTimerInfo info, CancellationToken cancellationToken)
    {
        logger.LogDebug("CreateSeriesTimerAsync called.");

        await this.dvrTimerModifyLock.WaitAsync(cancellationToken).ConfigureAwait(true);
        try
        {
            var timerInfos = Plugin.Instance!.Configuration.SeriesTimerInfos;
            var mutableTimerInfos = timerInfos.ToList();
            mutableTimerInfos.Add(info);
            Plugin.Instance!.Configuration.SeriesTimerInfos = mutableTimerInfos.ToImmutableArray();
        }
        finally
        {
            this.dvrTimerModifyLock.Release();
        }
    }

    /// <inheritdoc />
    public async Task UpdateTimerAsync(TimerInfo updatedTimer, CancellationToken cancellationToken)
    {
        logger.LogDebug("UpdateTimerAsync called.");

        await this.dvrTimerModifyLock.WaitAsync(cancellationToken).ConfigureAwait(true);
        try
        {
            var timerInfos = Plugin.Instance!.Configuration.TimerInfos;
            var maybeTarget = timerInfos.FirstOrDefault(tI => tI.Id == updatedTimer.Id);
            if (maybeTarget != null)
            {
                var mutableTimerInfos = timerInfos.ToList();
                mutableTimerInfos.Remove(maybeTarget);
                mutableTimerInfos.Add(new SerializableTimerInfo(updatedTimer));
                Plugin.Instance!.Configuration.TimerInfos = mutableTimerInfos.ToImmutableArray();
            }
        }
        finally
        {
            this.dvrTimerModifyLock.Release();
        }
    }

    /// <inheritdoc />
    public async Task UpdateSeriesTimerAsync(SeriesTimerInfo info, CancellationToken cancellationToken)
    {
        logger.LogDebug("UpdateSeriesTimerAsync called.");

        await this.dvrTimerModifyLock.WaitAsync(cancellationToken).ConfigureAwait(true);
        try
        {
            var timerInfos = Plugin.Instance!.Configuration.SeriesTimerInfos;
            var maybeTarget = timerInfos.FirstOrDefault(tI => tI.Id == info.Id);
            if (maybeTarget != null)
            {
                var mutableTimerInfos = timerInfos.ToList();
                mutableTimerInfos.Remove(maybeTarget);
                mutableTimerInfos.Add(info);
                Plugin.Instance!.Configuration.SeriesTimerInfos = mutableTimerInfos.ToImmutableArray();
            }
        }
        finally
        {
            this.dvrTimerModifyLock.Release();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TimerInfo>> GetTimersAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("GetTimersAsync called.");

        await this.dvrTimerModifyLock.WaitAsync(cancellationToken).ConfigureAwait(true);
        try
        {
            return Plugin.Instance!.Configuration.TimerInfos.Select(sTI => sTI.ToTimerInfo());
        }
        finally
        {
            this.dvrTimerModifyLock.Release();
        }
    }

    /// <inheritdoc />
    public Task<SeriesTimerInfo> GetNewTimerDefaultsAsync(CancellationToken cancellationToken, ProgramInfo program)
    {
        logger.LogDebug("GetNewTimerDefaultsAsync called.");

        SeriesTimerInfo seriesTimerInfo;

        // ignore the typing, it is actually nullable
        if (program == null)
        {
            seriesTimerInfo = new SeriesTimerInfo()
            {
                Id = Guid.NewGuid().ToString(),
                RecordAnyTime = true,
                RecordAnyChannel = false,
                SkipEpisodesInLibrary = false,
                RecordNewOnly = false,
                Priority = default(int),
                PrePaddingSeconds = 30,
                PostPaddingSeconds = 30,
                IsPrePaddingRequired = true,
                IsPostPaddingRequired = true,
                ServiceName = "TheTVApp",
            };
        }
        else
        {
            seriesTimerInfo = new SeriesTimerInfo()
            {
                Id = Guid.NewGuid().ToString(),
                RecordAnyTime = true,
                RecordAnyChannel = false,
                SkipEpisodesInLibrary = false,
                RecordNewOnly = false,
                Priority = default(int),
                PrePaddingSeconds = 30,
                PostPaddingSeconds = 30,
                IsPrePaddingRequired = true,
                IsPostPaddingRequired = true,
                ServiceName = "TheTVApp",

                ChannelId = program.ChannelId,
                ProgramId = program.Id,
                Name = program.Name ?? "Scheduled Programming",
                Overview = program.Overview ?? program.Name ?? "Scheduled Programming",
                StartDate = program.StartDate,
                EndDate = program.EndDate,
                SeriesId = program.Name?.IdOfString()
            };
        }

        return Task.FromResult(seriesTimerInfo);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SeriesTimerInfo>> GetSeriesTimersAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("GetSeriesTimersAsync called.");

        await this.dvrTimerModifyLock.WaitAsync(cancellationToken).ConfigureAwait(true);
        try
        {
            return Plugin.Instance!.Configuration.SeriesTimerInfos;
        }
        finally
        {
            this.dvrTimerModifyLock.Release();
        }
    }

    /// <inheritdoc />
    public Task ResetTuner(string id, CancellationToken cancellationToken)
    {
        logger.LogDebug("ResetTuner called.");

        // not necessary to implement
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task CloseLiveStream(string id, CancellationToken cancellationToken)
    {
        logger.LogDebug("CloseLiveStream called.");

        // not necessary to implement
        return Task.CompletedTask;
    }

    // private methods follow
    private async Task<IEnumerable<Uri>> GetStreamPageUrisAsync(Uri listingPageUri, CancellationToken cancellationToken)
    {
        logger.LogDebug("GetStreamPageUrisAsync called for {0}.", listingPageUri);

        using (var httpClient = this.httpClientFactory.CreateClient())
        {
            List<Uri> streamPageUris = new List<Uri>();

            HttpResponseMessage listingPageHttpResponseMessage = await httpClient.GetAsync(listingPageUri, cancellationToken).ConfigureAwait(false);
            listingPageHttpResponseMessage.EnsureSuccessStatusCode();
            string listingPageResponse = await listingPageHttpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            // parse stream page URIs (see re/README.md)
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(listingPageResponse);
            HtmlNode? olElement = doc.QuerySelector("body > div > div > div > ol");

            if (olElement == null)
            {
                logger.LogError("Could not identify stream page URIs on listing page {0}.", listingPageUri);
                throw new InvalidOperationException("Could not identify stream page URIs.");
            }

            foreach (HtmlNode liElement in olElement.ChildNodes)
            {
                // if this element is an <a>, we want to add its FULL href to the list
                if (liElement.Name == "a")
                {
                    string href = liElement.GetAttributeValue("href", string.Empty);
                    Uri streamPageUri = new Uri(listingPageUri, href);
                    streamPageUris.Add(streamPageUri);
                }
            }

            logger.LogDebug("GetStreamPageUrisAsync returning: {0}", streamPageUris);

            return streamPageUris;
        }
    }

    private async Task<TheTvAppChannel> GetTvAppChannelFromStreamPageUriAsync(Uri streamPageUri, CancellationToken cancellationToken)
    {
        logger.LogDebug("GetTvAppChannelFromStreamPageUriAsync called for {0}.", streamPageUri);

        using (var httpClient = this.httpClientFactory.CreateClient())
        {
            HttpResponseMessage streamPageHttpResponseMessage = await httpClient.GetAsync(streamPageUri, cancellationToken).ConfigureAwait(false);
            streamPageHttpResponseMessage.EnsureSuccessStatusCode();
            string streamPageResponse = await streamPageHttpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(streamPageResponse);

            // get the raw encrypted text
            HtmlNode? encryptedText = doc.QuerySelector("#encrypted-text");
            if (encryptedText == null)
            {
                logger.LogError("Could not identify encrypted text on stream page {0}.", streamPageUri);
                throw new InvalidOperationException("Could not identify encrypted text.");
            }

            string encryptedTextValue = encryptedText.Attributes["data"].Value;

            // because the key changes, we need to fetch the script and use it to decrypt the video URL
            HtmlNode? jsScriptElement = doc.QuerySelector("script[type='module']");
            if (jsScriptElement == null)
            {
                logger.LogError("Could not identify script element on stream page {0}.", streamPageUri);
                throw new InvalidOperationException("Could not identify script element.");
            }

            string jsScriptSourceUrl = jsScriptElement.Attributes["src"].Value;

            HttpResponseMessage jsScriptHttpResponseMessage = await httpClient.GetAsync(jsScriptSourceUrl, cancellationToken).ConfigureAwait(false);
            jsScriptHttpResponseMessage.EnsureSuccessStatusCode();
            string rawObfuscatedJsScriptResponse = await jsScriptHttpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            Uri? hlsUri = await this.videoDecryption.DecryptVideoUrl(encryptedTextValue, rawObfuscatedJsScriptResponse, cancellationToken).ConfigureAwait(false);

            if (hlsUri == null)
            {
                logger.LogError("Failed to decrypt video URL.");
                throw new InvalidOperationException("Failed to decrypt video URL.");
            }

            // in a Uri like "https://v11.thetvapp.to/hls/WNBCDT1/index.m3u8?token=OHcwUkwwbllzcVdDZWdGbzRkUExZRnhzQ1R0bEpCVkIwNGZLcExEaw=="
            // the callsign is "WNBCDT1"
            string callsign = hlsUri.Segments[^2].TrimEnd('/');

            string iso8601regex = @"\d{4}-(?:0[1-9]|1[0-2])-(?:0[1-9]|[12]\d|3[01])T(?:[01]\d|2[0-3]):[0-5]\d:[0-5]\d(?:\.\d+)?(?:Z|[+-](?:[01]\d|2[0-3]):[0-5]\d)";
            string guideRegex = @"https:\/\/thetvapp\.to\/json\/\d+\.json";

            Match dateMatch = Regex.Match(streamPageResponse, iso8601regex);
            Match guideMatch = Regex.Match(streamPageResponse, guideRegex);

            if (guideMatch.Success)
            {
                var guideUri = new Uri(guideMatch.Value);
                HttpResponseMessage guideHttpResponseMessage = await httpClient.GetAsync(guideUri, cancellationToken).ConfigureAwait(false);
                guideHttpResponseMessage.EnsureSuccessStatusCode();
                string guideResponse = await guideHttpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                if (guideResponse == null)
                {
                    logger.LogError("Could not retrieve guide response from {0}.", guideUri);
                    throw new InvalidOperationException("Could not retrieve guide response.");
                }

                HtmlNode? streamTitle = doc.QuerySelector("body > div > div > div.col-lg-8 > div.mb-3 > h2");

                if (streamTitle == null)
                {
                    logger.LogError("Could not identify stream title on stream page {0}.", streamPageUri);
                    throw new InvalidOperationException("Could not identify stream title.");
                }

                // parse the guide response JSON
                var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(guideResponse));
                using (memoryStream)
                {
                    var guideEntries = await JsonSerializer.DeserializeAsync<TvGuideEntry[]>(memoryStream, this.jsonSerializerOptions, cancellationToken).ConfigureAwait(false);

                    if (guideEntries == null)
                    {
                        logger.LogError("Could not deserialize guide response from {0}.", guideUri);
                        throw new InvalidOperationException("Could not deserialize guide response.");
                    }

                    var title = HttpUtility.HtmlDecode(streamTitle.InnerText ?? "Unknown Title");

                    return new TheTvAppChannel(title, callsign, hlsUri, null, guideEntries);
                }
            }
            else if (dateMatch.Success)
            {
                var startTimeUtc = DateTime.Parse(dateMatch.Value, null, DateTimeStyles.RoundtripKind);

                HtmlNode? streamTitle = doc.QuerySelector("body > div > div > div.col-lg-8 > div.mb-3 > h1");

                if (streamTitle == null)
                {
                    logger.LogError("Could not identify stream title on stream page {0}.", streamPageUri);
                    throw new InvalidOperationException("Could not identify stream title.");
                }

                var title = HttpUtility.HtmlDecode(streamTitle.InnerText ?? "Unknown Title");

                return new TheTvAppChannel(title, callsign, hlsUri, startTimeUtc, null);
            }
            else
            {
                logger.LogError("Could not identify guide or start time on stream page {0}.", streamPageUri);
                throw new InvalidOperationException("Could not identify guide or start time.");
            }
        }
    }

    private async Task<IEnumerable<TheTvAppChannel>> FetchNewTvAppChannelsAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("FetchNewTvAppChannelsAsync called.");

        ImmutableArray<string> channelListingStrings =
        [
            "https://thetvapp.to/tv",
            "https://thetvapp.to/nfl",
            "https://thetvapp.to/nba",
            "https://thetvapp.to/mlb",
            "https://thetvapp.to/nhl",
            "https://thetvapp.to/ncaaf",
        ];
        var channelListingUris = channelListingStrings.Select(s => new Uri(s)).ToList();
        var channelListingStreamPageUris = await TaskExtensions.WhenAllSuccessful(channelListingUris.Select(uri => this.GetStreamPageUrisAsync(uri, cancellationToken)), task => { logger.LogWarning("Could not parse a listing page, continuing anyway.\n{0}", task.Exception!.GetBaseException().ToString()); }).ConfigureAwait(false);
        var channelStreamPageUris = channelListingStreamPageUris.SelectMany(enumerable => enumerable).ToList();

        var appChannels = await TaskExtensions.WhenAllSuccessful(channelStreamPageUris.Select(uri => this.GetTvAppChannelFromStreamPageUriAsync(uri, cancellationToken)), task => { logger.LogWarning("Could not parse a streaming page, continuing anyway.\n{0}", task.Exception!.GetBaseException().ToString()); }).ConfigureAwait(false);

        // now that we have a list of app channels, if two or more channels declare the same callsign, prefer the one that is NOT marked as a live event
        var appChannelsByCallsign = appChannels.ToLookup(channel => channel.Callsign);
        return appChannelsByCallsign.Select(grouping => grouping.OrderByDescending(channel => channel.AppChannelType).First()).ToList();
    }

    private async Task<IEnumerable<TheTvAppChannel>> GetTvAppChannelsAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("GetTvAppChannelsAsync called.");

        await this.channelCacheLock.WaitAsync(cancellationToken).ConfigureAwait(true);
        try
        {
            if (this.tvAppChannelsCache == null || DateTime.UtcNow - this.lastFetchTime > this.cacheDuration)
            {
                this.tvAppChannelsCache = await this.FetchNewTvAppChannelsAsync(cancellationToken).ConfigureAwait(false);
                this.lastFetchTime = DateTime.UtcNow;
            }
        }
        finally
        {
            this.channelCacheLock.Release();
        }

        return this.tvAppChannelsCache;
    }
}
