using iPodcastSearch.Models;
using iPodcastSearch.Models.iTunes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace iPodcastSearch
{
  public class iTunesSearchClient : IPodcastSearchClient
  {
    private readonly string _baseLookupUrl = "https://itunes.apple.com/lookup?{0}";
    private readonly string _baseSearchUrl = "https://itunes.apple.com/search?{0}";
    private readonly string defaultLang = "us";
    private readonly int defaultResultLimit = 100;

        public Task<IList<Podcast>> SearchPodcastsAsync(string query)
        {
            return this.SearchPodcastsAsync(query, this.defaultResultLimit, this.defaultLang);
        }

        public Task<IList<Podcast>> SearchPodcastsAsync(string query, int resultLimit)
        {
            return this.SearchPodcastsAsync(query, resultLimit, this.defaultLang);
        }

        public async Task<IList<Podcast>> SearchPodcastsAsync(
      string query,
      int resultLimit,
      string countryCode)
    {
      HttpValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
      queryString.Add("term", query);
      queryString.Add("media", "podcast");
      queryString.Add("attribute", "titleTerm");
      queryString.Add("limit", resultLimit.ToString());
      queryString.Add("country", countryCode);
      return (IList<Podcast>) 
                (await this.MakeAPICall<PodcastListResult>(string.Format(this._baseSearchUrl, 
                (object) queryString))).Podcasts.Select<iTunesPodcast, Podcast>((Func<iTunesPodcast, Podcast>) (x => x.ToPodcast())).ToList<Podcast>();
    }

    public async Task<Podcast> GetPodcastByIdAsync(long podcastId, bool includeEpisodes = false)
    {
      HttpValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
      queryString.Add("id", podcastId.ToString());

      iTunesPodcast iTunesPodcast = 
                (await this.MakeAPICall<PodcastListResult>(
                    string.Format(this._baseLookupUrl, (object) queryString)))
                    .Podcasts.FirstOrDefault<iTunesPodcast>();

      Podcast podcast1 = iTunesPodcast != null ? iTunesPodcast.ToPodcast() : (Podcast) null;
      if (podcast1 != null & includeEpisodes)
      {
        Podcast podcast2 = podcast1;
        IList<PodcastEpisode> podcastEpisodesAsync 
                    = await this.GetPodcastEpisodesAsync(podcast1.FeedUrl);
        podcast2.Episodes = podcastEpisodesAsync;
        podcast2 = (Podcast) null;
        podcast1.EpisodeCount = podcast1.Episodes.Count;
      }
      return podcast1;
    }

        public async Task<IList<PodcastEpisode>> GetPodcastEpisodesAsync(string feedUrl)
        {
            return (await PodcastFeedParser.LoadFeedAsync(feedUrl)).Episodes;
        }

        public async Task<Podcast> GetPodcastFromFeedUrlAsyc(string feedUrl)
        {
            return await PodcastFeedParser.LoadFeedAsync(feedUrl);
        }

        private async Task<T> MakeAPICall<T>(string apiCall)
        {
            return JsonHelper.Deserialize<T>(await new HttpClient().GetStringAsync(apiCall).ConfigureAwait(false));
        }
    }
}
