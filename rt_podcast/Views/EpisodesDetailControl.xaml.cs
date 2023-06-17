using rt_podcast.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace rt_podcast.Views
{
    public sealed partial class EpisodesDetailControl : UserControl
    {
        public PodcastEpisodeModel MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as PodcastEpisodeModel; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem",typeof(PodcastEpisodeModel),typeof(EpisodesDetailControl),new PropertyMetadata(null));

        public EpisodesDetailControl()
        {
            InitializeComponent();
        }
    }
}
