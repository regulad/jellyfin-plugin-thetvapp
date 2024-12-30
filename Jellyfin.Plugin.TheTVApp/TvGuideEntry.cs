namespace Jellyfin.Plugin.TheTVApp;

using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Represents a single entry in the TV guide.
/// </summary>
public class TvGuideEntry
{
    /// <summary>
    /// Gets the JSON type info for this class that cam be used to serialize and deserialize objects of this type.
    /// </summary>
    public static JsonTypeInfo<TvGuideEntry[]> ThisJsonTypeInfo => JsonTypeInfo.CreateJsonTypeInfo<TvGuideEntry[]>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

    /// <summary>
    /// Gets or sets the title of production.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the title of the episode of the production. May be null.
    /// </summary>
    public string? EpisodeTitle { get; set; }

    /// <summary>
    /// Gets or sets the start time of the production in UTC timestamp.
    /// </summary>
    public required int StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the production in UTC timestamp.
    /// </summary>
    public required int EndTime { get; set; }
}
