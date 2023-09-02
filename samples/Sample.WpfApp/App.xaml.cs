using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sample.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Ioc.Default.AddCore(services => ConfigureServices(services));
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var shell = Ioc.Default.GetRequiredService<MainWindow>();
            shell.Show();
        }

        private IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services;
        }
    }
}
