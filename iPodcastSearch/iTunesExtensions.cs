using iPodcastSearch.Models;
using iPodcastSearch.Models.iTunes;
using System;
using System.Xml.Linq;

namespace iPodcastSearch
{
  public static class iTunesExtensions
  {
    public static bool IsExplicit(this XElement element) => element.GetStringFromItunes("explicit").IsExplicit();

    public static bool IsExplicit(this string value) => string.Equals(value, "yes", StringComparison.CurrentCultureIgnoreCase);

    public static DateTime ToDateTime(this string value)
    {
      DateTime result = DateTime.MinValue;
      DateTime.TryParse(value, out result);
      return result;
    }

    public static Podcast ToPodcast(this iTunesPodcast iTunesPodcast) => new Podcast()
    {
      Name = iTunesPodcast.Name,
      Author = iTunesPodcast.ArtistName,
      Id = iTunesPodcast.Id,
      EpisodeCount = iTunesPodcast.EpisodeCount,
      Copyright = iTunesPodcast.Copyright,
      Description = iTunesPodcast.Description,
      FeedUrl = iTunesPodcast.FeedUrl,
      IsExplicit = iTunesPodcast.Explicitness.IsExplicit(),
      Image = iTunesPodcast.ArtworkUrlLarge,
      SubTitle = iTunesPodcast.ReleaseDate,
      LastUpdate = iTunesPodcast.ReleaseDate.ToDateTime(),
      Language = iTunesPodcast.Country,
      Website = iTunesPodcast.PodcastViewUrl
    };
  }
}
