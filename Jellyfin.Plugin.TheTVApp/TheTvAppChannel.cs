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
            Number = this.Callsign.IdOfString(),
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

            yield return programInfo;
        }
        else if (this.GuideEntries != null)
        {
            foreach (var guideEntry in this.GuideEntries!)
            {
                var programInfo = new ProgramInfo
                {
                    Id = (channelInfo.CallSign + guideEntry.StartTime.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)).IdOfString(),
                    ChannelId = channelInfo.Id,
                    Name = guideEntry.Title,
                    EpisodeTitle = guideEntry.EpisodeTitle,
                    StartDate = DateTimeOffset.FromUnixTimeSeconds(guideEntry.StartTime).UtcDateTime,
                    EndDate = DateTimeOffset.FromUnixTimeSeconds(guideEntry.EndTime).UtcDateTime,
                    Genres = new List<string> { "Live" },
                };

                yield return programInfo;
            }
        }
    }

    /// <summary>
    /// Returns a collection of <see cref="MediaSourceInfo"/> objects representing the media sources for this channel or sporting event.
    /// </summary>
    /// <param name="httpClient"> The HTTP client to use to download the HLS content. </param>
    /// <returns>
    /// The collection.
    /// </returns>
    public async Task<IEnumerable<MediaSourceInfo>> ToMediaSourceInfosAsync(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        HttpResponseMessage response = await httpClient.GetAsync(this.HlsUri).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        string hlsContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        // we can expect hlsContent to look something like:
        //
        // #EXTM3U
        // #EXT-X-STREAM-INF:AVERAGE-BANDWIDTH=1320000,BANDWIDTH=1650000,RESOLUTION=640x360,FRAME-RATE=59.940,CODECS="avc1.64001f,mp4a.40.2",CLOSED-CAPTIONS=NONE
        // tracks-v2a1/mono.m3u8?token=OHcwUkwwbllzcVdDZWdGbzRkUExZRnhzQ1R0bEpCVkIwNGZLcExEaw%3D%3D
        // #EXT-X-STREAM-INF:AVERAGE-BANDWIDTH=2860000,BANDWIDTH=3580000,RESOLUTION=1280x720,FRAME-RATE=59.940,CODECS="avc1.4d4028,mp4a.40.2",CLOSED-CAPTIONS=NONE
        // tracks-v1a1/mono.m3u8?token=OHcwUkwwbllzcVdDZWdGbzRkUExZRnhzQ1R0bEpCVkIwNGZLcExEaw%3D%3D
        //
        // each one of the lines starting with #EXT-X-STREAM-INF is a different quality level of the stream, and will be a separate MediaSourceInfo
        var mediaSourceInfos = new List<MediaSourceInfo>();

        var lines = hlsContent.Split('\n').ToImmutableList();

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
                        break;
                    }

                    var infoDict = new Dictionary<string, string>();
                    foreach (var pair in line.Split(":")[1].Split(','))
                    {
                        var key = pair.Split('=')[0];
                        var value = pair.Split('=')[1];
                        infoDict.Add(key, value);
                    }

                    var mediaSourceInfo = new MediaSourceInfo
                    {
                        Protocol = MediaProtocol.Http,
                        Id = nextLine.IdOfString(),
                        Path = nextLine,
                        Name = this.Name,
                        IsRemote = true,
                        SupportsProbing = false,
                        SupportsDirectStream = false,
                        Container = "mpegts",
                        RequiresOpening = true,
                        SupportsTranscoding = true,
                        MediaStreams = new List<MediaStream>
                        {
                            new MediaStream
                            {
                                Type = MediaStreamType.Video,
                                IsInterlaced = false,

                                // Set the index to -1 because we don't know the exact index of the video stream within the container
                                Index = -1,

                                // FRAME-RATE
                                AverageFrameRate = float.Parse(infoDict["FRAME-RATE"], CultureInfo.InvariantCulture),

                                // RESOLUTION
                                Width = int.Parse(infoDict["RESOLUTION"].Split('x')[0], CultureInfo.InvariantCulture),
                                Height = int.Parse(infoDict["RESOLUTION"].Split('x')[1], CultureInfo.InvariantCulture),

                                // CODECS
                                Codec = infoDict["CODECS"].Split(',')[0],
                            },
                            new MediaStream
                            {
                                Type = MediaStreamType.Audio,

                                // Set the index to -1 because we don't know the exact index of the audio stream within the container
                                Index = -1,

                                // CODECS
                                Codec = infoDict["CODECS"].Split(',')[1],
                            },
                        },
                        Bitrate = int.Parse(infoDict["BANDWIDTH"], CultureInfo.InvariantCulture),
                    };

                    mediaSourceInfos.Add(mediaSourceInfo);
                }
            }
        }

        return mediaSourceInfos;
    }
}
