using CommunityToolkit.Mvvm.ComponentModel;

namespace Sample.WpfDevelopers.WpfApp.ViewModels
{
    internal class HomePageViewModel : ObservableObject
    {
        public string? Title
        {
            get
            {
                return "Home";
            }
        }
    }
}
