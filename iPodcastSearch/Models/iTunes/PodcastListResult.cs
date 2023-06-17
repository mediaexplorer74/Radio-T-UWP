using System.Collections.Generic;
using System.Runtime.Serialization;

namespace iPodcastSearch.Models.iTunes
{
  [DataContract]
  public class PodcastListResult
  {
    [DataMember(Name = "resultCount")]
    public int Count { get; set; }

    [DataMember(Name = "results")]
    public List<iTunesPodcast> Podcasts { get; set; }
  }
}
