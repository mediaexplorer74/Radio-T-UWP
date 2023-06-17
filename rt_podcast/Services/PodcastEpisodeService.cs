using System.Collections.Generic;
using System.Threading.Tasks;

using rt_podcast.Models;

using iPodcastSearch;
using System.Linq;
using Windows.UI.Xaml.Controls;
using System.Globalization;
using System;

namespace rt_podcast.Services
{
    public class PodcastEpisodeService
    {


        // TimeToStr function produces the following output:
        //    In Invariant Language (Invariant Country), 05/01/2009 09:00:00
        //    In en-US, 5/1/2009 9:00:00 AM
        //    In fr-FR, 01/05/2009 09:00:00
        //    In de-DE, 01.05.2009 09:00:00
        //    In es-ES, 01/05/2009 9:00:00
        //    In ja-JP, 2009/05/01 9:00:00
        public string TimeToStr(DateTime s)
        {
            
            CultureInfo culture = CultureInfo.InvariantCulture;
            //                                       new CultureInfo("en-us"),
            //                                       new CultureInfo("fr-fr"),
            //                                       new CultureInfo("de-DE"),
            //                                       new CultureInfo("es-ES"),
            //                                       new CultureInfo("ja-JP")};

            DateTime thisDate = s;//new DateTime(2009, 5, 1, 9, 0, 0);

            //foreach (CultureInfo culture in cultures)
            //{
                string cultureName;
                
            //if (string.IsNullOrEmpty(culture.Name))
            //        cultureName = culture.NativeName;
            //    else
                    cultureName = culture.Name;

             //   Console.WriteLine("In {0}, {1}",
             //                     cultureName, thisDate.ToString(culture));
             //}
             return thisDate.ToString(culture);
        }
    
   



        public async Task<IEnumerable<PodcastEpisodeModel>> GetDataAsync()
        {
            var feedUrl = "http://feeds.rucast.net/radio-t";//"https://www.codingblocks.net/podcast-feed.xml";
            var podcast = await PodcastFeedParser.LoadFeedAsync(feedUrl);
            
            var data = podcast.Episodes.Select(x => new PodcastEpisodeModel
            {
                Title = x.Title,
                Description = TimeToStr(x.PubDate),//x.PubDate.ToString(),
                Summary = x.Summary,
                Symbol = Symbol.Globe,
                MediaFileUrl = x.AudioFileUrl
            });

            return data;
        }
    }
}
