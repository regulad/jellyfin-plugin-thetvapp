using System.Linq;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.TheTVApp;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using MediaBrowser.Controller.LiveTv;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.LiveTv;
using MediaBrowser.Model.MediaInfo;

/// <summary>
/// Represents a single channel or sporting event on thetvapp.to.
/// </summary>
public class TheTvAppChannel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TheTvAppChannel"/> class.
    /// </summary>
    /// <param name="name"> The name of the channel or sporting event. </param>
    /// <param name="callsign"> The callsign of the channel or sporting event. </param>
    /// <param name="hlsUri"> The HLS URI of the channel or sporting event. </param>
    /// <param name="startTimeUtc"> The start time of the channel or sporting event. </param>
    /// <param name="guideEntries"> The guide entries for the channel or sporting event. </param>
    /// <exception cref="ArgumentException"> Thrown when the channel type is invalid. </exception>
    public TheTvAppChannel(string name, string callsign, Uri hlsUri, DateTime? startTimeUtc, IEnumerable<TvGuideEntry>? guideEntries)
    {
        this.Name = name;
        this.Callsign = callsign;
        this.HlsUri = hlsUri;
        this.StartTimeUtc = startTimeUtc;
        this.GuideEntries = guideEntries;

        if (startTimeUtc != null && guideEntries == null)
        {
            this.AppChannelType = TvAppChannelType.LiveEvent;
        }
        else if (startTimeUtc == null && guideEntries != null)
        {
            this.AppChannelType = TvAppChannelType.Tv;
        }
        else
        {
            throw new ArgumentException("Invalid channel type.");
        }
    }

    /// <summary>
    /// The channel type.
    /// </summary>
    public enum TvAppChannelType
    {
        /// <summary>
        /// A normal TV channel. This channel has a guide, but no start time.
        /// </summary>
        Tv,

        /// <summary>
        /// A live event, normally a sporting event. Has a start time, but no guide.
        /// </summary>
        LiveEvent,
    }

    /// <summary>
    /// Gets the name of the channel or sporting event as seen on thetvapp.to.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the callsign of the channel or sporting event as seen on thetvapp.to.
    /// </summary>
    public string Callsign { get; }

    /// <summary>
    /// Gets the HLS (HTTP Live Stream) URI of the channel or sporting event as seen on thetvapp.to.
    /// </summary>
    public Uri HlsUri { get; }

    /// <summary>
    /// Gets the start time of the channel or sporting event as seen on thetvapp.to.
    /// </summary>
    public DateTime? StartTimeUtc { get; }

    /// <summary>
    /// Gets the guide entries for the channel or sporting event as seen on thetvapp.to.
    /// </summary>
    public IEnumerable<TvGuideEntry>? GuideEntries { get; }

    /// <summary>
    /// Gets the type of channel or sporting event.
    /// </summary>
    public TvAppChannelType AppChannelType { get; }

    /// <summary>
    /// Returns a <see cref="ChannelInfo"/> object representing this channel or sporting event.
    /// </summary>
    /// <returns> The <see cref="ChannelInfo"/>. </returns>
    public ChannelInfo ToChannelInfo()
    {
        return new ChannelInfo
        {
            Name = this.Name,
            Number = this.Callsign,
            Id = this.Callsign,
            TunerChannelId = this.Callsign,
            CallSign = this.Callsign,
            ChannelType = ChannelType.TV,
            ChannelGroup = "TheTVApp",
            ImageUrl = "https://thetvapp.to/img/TheTVApp.svg",
            HasImage = true,
        };
    }

    /// <summary>
    /// Returns a collection of <see cref="ProgramInfo"/> objects representing the programs on this channel or sporting event.
    /// </summary>
    /// <returns> The collection. </returns>
    public IEnumerable<ProgramInfo> ToProgramInfos()
    {
        var channelInfo = this.ToChannelInfo();

        if (this.StartTimeUtc.HasValue)
        {
            var programInfo = new ProgramInfo
            {
                Id = (channelInfo.CallSign + this.StartTimeUtc.Value.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)).IdOfString(),
                ChannelId = channelInfo.Id,
                Name = this.Name,
                StartDate = this.StartTimeUtc.Value,
                EndDate = this.StartTimeUtc.Value.AddHours(3),
                IsSports = true,
                Genres = new List<string> { "Live" },
            };

            return new List<ProgramInfo>() { programInfo };
        }
        else if (this.GuideEntries != null)
        {
            var programInfos = new List<ProgramInfo>();

            foreach (var guideEntry in this.GuideEntries!)
            {
                var startTime = DateTimeOffset.FromUnixTimeSeconds(guideEntry.StartTime).UtcDateTime;
                var endTime = DateTimeOffset.FromUnixTimeSeconds(guideEntry.EndTime).UtcDateTime;

                var programInfo = new ProgramInfo
                {
                    Id = (channelInfo.CallSign + startTime.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)).IdOfString(),
                    ChannelId = channelInfo.Id,
                    Name = guideEntry.Title,
                    EpisodeTitle = guideEntry.EpisodeTitle,
                    StartDate = startTime,
                    EndDate = endTime,
                    IsLive = true,
                    Genres = new List<string> { "Live" },
                };

                programInfos.Add(programInfo);
            }

            return programInfos;
        }
        else
        {
            return Array.Empty<ProgramInfo>();
        }
    }

    /// <summary>
    /// Returns a collection of <see cref="MediaSourceInfo"/> objects representing the media sources for this channel or sporting event.
    /// </summary>
    /// <param name="logger"> The logger to use to log messages. </param>
    /// <param name="httpClient"> The HTTP client to use to download the HLS content. </param>
    /// <returns>
    /// The collection.
    /// </returns>
    public async Task<IEnumerable<MediaSourceInfo>> ToMediaSourceInfosAsync(ILogger<LiveTvService> logger, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(logger);

        try
        {
            logger.LogInformation("Fetching HLS content from {Uri}", this.HlsUri);
            HttpResponseMessage response = await httpClient.GetAsync(this.HlsUri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            string hlsContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var mediaSourceInfos = new List<MediaSourceInfo>();
            var lines = hlsContent.Split('\n').ToImmutableList();

            logger.LogDebug("Processing {LineCount} lines of HLS content", lines.Count);

            using (var enumerator = lines.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var line = enumerator.Current;

                    if (line.StartsWith("#EXT-X-STREAM-INF", StringComparison.Ordinal))
                    {
                        var nextLine = enumerator.MoveNext() ? enumerator.Current : null;
                        if (nextLine == null)
                        {
                            logger.LogWarning("Found stream info line without corresponding stream URL");
                            break;
                        }

                        try
                        {
                            var mediaSourceInfo = ParseStreamInfo(line, nextLine, logger);
                            if (mediaSourceInfo != null)
                            {
                                mediaSourceInfos.Add(mediaSourceInfo);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Failed to parse stream info from line: {Line}", line);
                        }
                    }
                }
            }

            if (mediaSourceInfos.Count == 0)
            {
                logger.LogWarning("No valid media sources found in HLS content");
            }
            else
            {
                logger.LogInformation("Successfully parsed {Count} media sources", mediaSourceInfos.Count);
            }

            return mediaSourceInfos;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Failed to fetch HLS content from {Uri}", this.HlsUri);
            throw;
        }
    }

    private MediaSourceInfo? ParseStreamInfo(string infoLine, string streamUrl, ILogger logger)
    {
        var infoDict = new Dictionary<string, string>();

        try
        {
            var infoParts = infoLine.Split(":", 2);
            if (infoParts.Length != 2)
            {
                logger.LogWarning("Invalid stream info format: {Line}", infoLine);
                return null;
            }

            foreach (var pair in infoParts[1].Split(','))
            {
                var keyValue = pair.Split('=', 2);
                if (keyValue.Length == 2)
                {
                    infoDict[keyValue[0].Trim()] = keyValue[1].Trim(' ', '"');
                }
            }

            var mediaStreams = new List<MediaStream>();

            // Add video stream if we can parse the required information
            if (TryParseVideoStream(infoDict, out var videoStream, logger))
            {
                mediaStreams.Add(videoStream);
            }

            // Add audio stream if we can parse the codec
            if (TryParseAudioStream(infoDict, out var audioStream, logger))
            {
                mediaStreams.Add(audioStream);
            }

            var mediaSourceInfo = new MediaSourceInfo
            {
                Protocol = MediaProtocol.Http,
                Id = streamUrl.IdOfString(),
                Path = new Uri(this.HlsUri, streamUrl).ToString(),
                Name = this.Name,
                IsRemote = true,
                SupportsProbing = false,
                SupportsDirectStream = false,
                Container = "mpegts",
                RequiresOpening = true,
                SupportsTranscoding = true,
                MediaStreams = mediaStreams,
            };

            // Try parse bitrate
            if (infoDict.TryGetValue("BANDWIDTH", out var bandwidthStr))
            {
                if (int.TryParse(bandwidthStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var bandwidth))
                {
                    mediaSourceInfo.Bitrate = bandwidth;
                }
                else
                {
                    logger.LogWarning("Failed to parse bandwidth value: {Bandwidth}", bandwidthStr);
                }
            }

            return mediaSourceInfo;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error parsing stream info dictionary");
            return null;
        }
    }

    private bool TryParseVideoStream(Dictionary<string, string> infoDict, out MediaStream videoStream, ILogger logger)
    {
        videoStream = new MediaStream
        {
            Type = MediaStreamType.Video,
            IsInterlaced = false,
            Index = -1,
        };

        try
        {
            // Parse frame rate
            if (infoDict.TryGetValue("FRAME-RATE", out var frameRateStr))
            {
                if (float.TryParse(frameRateStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var frameRate))
                {
                    videoStream.AverageFrameRate = frameRate;
                }
                else
                {
                    logger.LogWarning("Failed to parse frame rate: {FrameRate}", frameRateStr);
                }
            }

            // Parse resolution
            if (infoDict.TryGetValue("RESOLUTION", out var resolutionStr))
            {
                var dimensions = resolutionStr.Split('x');
                if (dimensions.Length == 2)
                {
                    if (int.TryParse(dimensions[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var width))
                    {
                        videoStream.Width = width;
                    }

                    if (int.TryParse(dimensions[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var height))
                    {
                        videoStream.Height = height;
                    }
                }
                else
                {
                    logger.LogWarning("Invalid resolution format: {Resolution}", resolutionStr);
                }
            }

            // Parse video codec
            if (infoDict.TryGetValue("CODECS", out var codecsStr))
            {
                var codecs = codecsStr.Split(',');
                if (codecs.Length > 0)
                {
                    videoStream.Codec = codecs[0];
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error parsing video stream information");
            return false;
        }
    }

    private bool TryParseAudioStream(Dictionary<string, string> infoDict, out MediaStream audioStream, ILogger logger)
    {
        audioStream = new MediaStream
        {
            Type = MediaStreamType.Audio,
            Index = -1,
        };

        try
        {
            if (infoDict.TryGetValue("CODECS", out var codecsStr))
            {
                var codecs = codecsStr.Split(',');
                if (codecs.Length > 1)
                {
                    audioStream.Codec = codecs[1];
                    return true;
                }
            }

            logger.LogWarning("No audio codec information found in stream info");
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error parsing audio stream information");
            return false;
        }
    }
}
