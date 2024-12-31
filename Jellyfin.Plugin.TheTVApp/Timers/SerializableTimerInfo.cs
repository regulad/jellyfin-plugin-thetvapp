#pragma warning disable

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MediaBrowser.Controller.LiveTv;
using MediaBrowser.Model.LiveTv;

namespace Jellyfin.Plugin.TheTVApp.Timers;

/// <summary>
/// A serializable version of <see cref="TimerInfo"/>.
/// </summary>
public class SerializableTimerInfo
{
   public string Id { get; set; }
   public string SeriesTimerId { get; set; }
   public string ChannelId { get; set; }
   public string ProgramId { get; set; }
   public string ShowId { get; set; }
   public string Name { get; set; }
   public string Overview { get; set; }
   public string SeriesId { get; set; }
   public DateTime StartDate { get; set; }
   public DateTime EndDate { get; set; }
   public RecordingStatus Status { get; set; }
   public int PrePaddingSeconds { get; set; }
   public int PostPaddingSeconds { get; set; }
   public bool IsPrePaddingRequired { get; set; }
   public bool IsPostPaddingRequired { get; set; }
   public bool IsManual { get; set; }
   public int Priority { get; set; }
   public int RetryCount { get; set; }
   public int? SeasonNumber { get; set; }
   public int? EpisodeNumber { get; set; }
   public bool IsMovie { get; set; }
   public bool IsSeries { get; set; }
   public int? ProductionYear { get; set; }
   public string EpisodeTitle { get; set; }
   public DateTime? OriginalAirDate { get; set; }
   public bool IsProgramSeries { get; set; }
   public bool IsRepeat { get; set; }
   public string HomePageUrl { get; set; }
   public float? CommunityRating { get; set; }
   public string OfficialRating { get; set; }
   public string[] Genres { get; set; }
   public string RecordingPath { get; set; }
   public KeepUntil KeepUntil { get; set; }
   public string[] Tags { get; set; }

   public SerializableTimerInfo()
   {

   }

   public SerializableTimerInfo(TimerInfo original)
   {
       Id = original.Id;
       SeriesTimerId = original.SeriesTimerId;
       ChannelId = original.ChannelId;
       ProgramId = original.ProgramId;
       ShowId = original.ShowId;
       Name = original.Name;
       Overview = original.Overview;
       SeriesId = original.SeriesId;
       StartDate = original.StartDate;
       EndDate = original.EndDate;
       Status = original.Status;
       PrePaddingSeconds = original.PrePaddingSeconds;
       PostPaddingSeconds = original.PostPaddingSeconds;
       IsPrePaddingRequired = original.IsPrePaddingRequired;
       IsPostPaddingRequired = original.IsPostPaddingRequired;
       IsManual = original.IsManual;
       Priority = original.Priority;
       RetryCount = original.RetryCount;
       SeasonNumber = original.SeasonNumber;
       EpisodeNumber = original.EpisodeNumber;
       IsMovie = original.IsMovie;
       IsSeries = original.IsSeries;
       ProductionYear = original.ProductionYear;
       EpisodeTitle = original.EpisodeTitle;
       OriginalAirDate = original.OriginalAirDate;
       IsProgramSeries = original.IsProgramSeries;
       IsRepeat = original.IsRepeat;
       HomePageUrl = original.HomePageUrl;
       CommunityRating = original.CommunityRating;
       OfficialRating = original.OfficialRating;
       Genres = original.Genres;
       RecordingPath = original.RecordingPath;
       KeepUntil = original.KeepUntil;
       Tags = original.Tags;
   }


   public TimerInfo ToTimerInfo()
   {
       return new TimerInfo
       {
           Id = Id,
           SeriesTimerId = SeriesTimerId,
           ChannelId = ChannelId,
           ProgramId = ProgramId,
           ShowId = ShowId,
           Name = Name,
           Overview = Overview,
           SeriesId = SeriesId,
           StartDate = StartDate,
           EndDate = EndDate,
           Status = Status,
           PrePaddingSeconds = PrePaddingSeconds,
           PostPaddingSeconds = PostPaddingSeconds,
           IsPrePaddingRequired = IsPrePaddingRequired,
           IsPostPaddingRequired = IsPostPaddingRequired,
           IsManual = IsManual,
           Priority = Priority,
           RetryCount = RetryCount,
           SeasonNumber = SeasonNumber,
           EpisodeNumber = EpisodeNumber,
           IsMovie = IsMovie,
           IsSeries = IsSeries,
           ProductionYear = ProductionYear,
           EpisodeTitle = EpisodeTitle,
           OriginalAirDate = OriginalAirDate,
           IsProgramSeries = IsProgramSeries,
           IsRepeat = IsRepeat,
           HomePageUrl = HomePageUrl,
           CommunityRating = CommunityRating,
           OfficialRating = OfficialRating,
           Genres = Genres,
           RecordingPath = RecordingPath,
           KeepUntil = KeepUntil,
           Tags = Tags
       };
   }
}
