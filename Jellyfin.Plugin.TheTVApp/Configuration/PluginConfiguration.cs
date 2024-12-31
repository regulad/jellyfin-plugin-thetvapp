using System;
using System.Collections.Immutable;
using Jellyfin.Plugin.TheTVApp.Timers;
using MediaBrowser.Controller.LiveTv;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.TheTVApp.Configuration;

/// <summary>
/// Plugin configuration.
/// </summary>
public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginConfiguration"/> class.
    /// </summary>
    public PluginConfiguration()
    {
        // set default options here
        OpenAiApiKey = null;
        TimerInfos = ImmutableArray<SerializableTimerInfo>.Empty;
        SeriesTimerInfos = ImmutableArray<SeriesTimerInfo>.Empty;
    }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string? OpenAiApiKey { get; set; }

    /// <summary>
    /// Gets or sets stored <see cref="TimerInfo"/>s. <seealso cref="SeriesTimerInfos"/>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1819", Justification = "Array is serialized")]
    public ImmutableArray<SerializableTimerInfo> TimerInfos { get; set; }

    /// <summary>
    /// Gets or sets stored <see cref="SeriesTimerInfo"/>s. <seealso cref="TimerInfo"/>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1819", Justification = "Array is serialized")]
    public ImmutableArray<SeriesTimerInfo> SeriesTimerInfos { get; set; }
}
