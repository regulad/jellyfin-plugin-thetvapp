namespace Jellyfin.Plugin.TheTVApp;

/// <summary>
/// Represents a single entry in the TV guide.
/// </summary>
public class TvGuideEntry
{
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
