using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gs.LOL;
using System.Threading.Tasks;

namespace Sample.WpfDevelopers.LOL.ViewModels
{
    internal partial class HomePageViewModel : ObservableObject
    {
        private readonly LeagueClient _league;

        [ObservableProperty]
        private LoginSession _loginSession;
        [ObservableProperty]
        private Summoner _summoner;
        public HomePageViewModel(LeagueClient league)
        {
            _league = league;
        }

        [RelayCommand]
        private async Task LoadAsync()
        {
            LoginSession = await _league.LoginSessionGetAsync();
            Summoner = await _league.SummonerGetAsync();
        }
    }
}
