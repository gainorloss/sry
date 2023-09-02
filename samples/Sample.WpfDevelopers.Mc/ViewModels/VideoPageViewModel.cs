using CommunityToolkit.Mvvm.ComponentModel;

namespace Sample.WpfDevelopers.WpfApp.ViewModels
{
    internal class VideoPageViewModel : ObservableObject
    {
        public string? Title
        {
            get
            {
                return "Video Page";
            }
        }
    }
}
