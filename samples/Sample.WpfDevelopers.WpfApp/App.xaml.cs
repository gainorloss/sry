using CommunityToolkit.Mvvm.DependencyInjection;
using Sample.WpfDevelopers.WpfApp.Views;
using System.Windows;

namespace Sample.WpfDevelopers.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Ioc.Default.AddCore(services => services
            //.AddLeagueClient()
            );
              
        }



        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Ioc.Default.GetRequiredService<MainWindow>().Show();
        }
    }
}
