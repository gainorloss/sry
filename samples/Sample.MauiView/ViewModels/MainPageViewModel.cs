using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sample.MauiView.Views;

namespace Sample.MauiView.ViewModels
{
    internal partial class MainPageViewModel : ObservableObject
    {
        public MainPageViewModel()
        {
        }

        [RelayCommand]
        public async Task NavigateAsync()
        {
            await Shell.Current.Navigation.PushAsync(new HelloXamlPage());
        }
    }
}