using iPodcastSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace iPodcastSearch
{
  public static class PodcastFeedParser
  {
    public static async Task<Podcast> LoadFeedAsync(string url)
    {
      Podcast feed = PodcastFeedParser.ParseFeed(await new HttpClient().GetStringAsync(url));
      feed.FeedUrl = url;
      return feed;
    }

    public static Podcast ParseFeed(string xml)
    {
      XDocument xmlRoot = XDocument.Parse(xml);
      Podcast podcastMetadata = PodcastFeedParser.ExtractPodcastMetadata(xmlRoot);
      IList<PodcastEpisode> episodes = PodcastFeedParser.ExtractEpisodes(xmlRoot);
      podcastMetadata.Episodes = episodes;
      podcastMetadata.EpisodeCount = episodes.Count;
      return podcastMetadata;
    }

    private static IList<PodcastEpisode> ExtractEpisodes(XDocument xmlRoot)
    {
      List<PodcastEpisode> episodes = new List<PodcastEpisode>();
      foreach (XElement descendant in xmlRoot.Descendants((XName) "item"))
      {
        try
        {
          string str1 = descendant.GetString("title");
          string stringFromItunes1 = descendant.GetStringFromItunes("subtitle");
          string stringFromItunes2 = descendant.GetStringFromItunes("author");
          string str2 = descendant.GetString("description");
          bool flag = descendant.IsExplicit();
          string stringFromItunes3 = descendant.GetStringFromItunes("image");
          string stringFromItunes4 = descendant.GetStringFromItunes("duration");
          string str3 = descendant.GetString("link");
          DateTime dateTime = descendant.GetDateTime("pubDate");
          string stringFromItunes5 = descendant.GetStringFromItunes("summary");
          XElement element1 = descendant.Element((XName) "enclosure");
          string attribute1 = element1 != null ? element1.ExtractAttribute("href") : (string) null;
          if (string.IsNullOrEmpty(attribute1))
          {
            XElement element2 = descendant.Element((XName) "enclosure");
            attribute1 = element2 != null ? element2.ExtractAttribute("url") : (string) null;
          }
          XElement element3 = descendant.Element((XName) "enclosure");
          string attribute2 = element3 != null ? element3.ExtractAttribute("length") : (string) null;
          XElement element4 = descendant.Element((XName) "enclosure");
          string attribute3 = element4 != null ? element4.ExtractAttribute("type") : (string) null;
          episodes.Add(new PodcastEpisode()
          {
            Title = str1,
            Author = stringFromItunes2,
            Description = str2,
            AudioFileUrl = attribute1,
            AudioFileSize = attribute2,
            AudioFileType = attribute3,
            IsExplicit = flag,
            Image = stringFromItunes3,
            Link = str3,
            PubDate = dateTime,
            Subtitle = stringFromItunes1,
            Summary = stringFromItunes5,
            Duration = stringFromItunes4
          });
        }
        catch (Exception ex)
        {
          string message = ex.Message;
          throw;
        }
      }
      return (IList<PodcastEpisode>) episodes;
    }

    private static Podcast ExtractPodcastMetadata(XDocument xmlRoot)
    {
      Podcast podcastMetadata = new Podcast();
      XElement xelement = xmlRoot != null ? xmlRoot.Descendants((XName) "channel").FirstOrDefault<XElement>() : (XElement) null;
      string str1 = xelement != null ? xelement.GetString("title") : throw new Exception("Incorrect XML");
      string str2 = xelement.GetString("link");
      string str3 = xelement.GetString("lastBuildDate");
      string str4 = xelement.GetString("description");
      string str5 = xelement.GetString("language");
      string str6 = xelement.GetString("copyright");
      string imageUrl = xelement.GetImageUrl();
      string stringFromItunes1 = xelement.GetStringFromItunes("subtitle");
      string stringFromItunes2 = xelement.GetStringFromItunes("author");
      bool flag = xelement.IsExplicit();
      string stringFromItunes3 = xelement.GetStringFromItunes("summary");
      string stringFromAtom = xelement.GetStringFromAtom("link");
      podcastMetadata.Language = str5;
      podcastMetadata.Author = stringFromItunes2;
      podcastMetadata.Image = imageUrl;
      podcastMetadata.Copyright = str6;
      podcastMetadata.Name = str1;
      podcastMetadata.Description = !string.IsNullOrWhiteSpace(stringFromItunes3) ? stringFromItunes3 : str4;
      podcastMetadata.IsExplicit = flag;
      podcastMetadata.FeedUrl = stringFromAtom;
      podcastMetadata.Website = str2;
      podcastMetadata.LastUpdate = str3.ToDateTime();
      podcastMetadata.SubTitle = stringFromItunes1;
      return podcastMetadata;
    }
  }
}
