using AutoUpdaterDotNET;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Sample.WpfDevelopers.WpfApp
{
    internal partial class TestPageViewModel : ObservableObject
    {
        public ObservableCollection<UserModel> UserCollection { get; set; }
        public TestPageViewModel()
        {
            UserCollection = new ObservableCollection<UserModel>();
            var time = DateTime.Now;
            for (int i = 0; i < 4; i++)
            {
                UserCollection.Add(new UserModel
                {
                    Date = time,
                    Name = "WPFDevelopers",
                    Address = "No. 189, Grove St, Los Angeles",

                });
                time = time.AddDays(2);
            }

            Clipboard.SetText("galoS");
            var cp = Clipboard.GetText();
        }
        public string BaseDir => AppContext.BaseDirectory;
        private string DownloadPath
        {

            get
            {
                var dir = AppContext.BaseDirectory;
                var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);
                if (currentDirectory.Parent != null)
                     dir = currentDirectory.Parent.FullName;
                return dir;
            }
        }
        [RelayCommand]
        public void Update()
        {
            AutoUpdater.DownloadPath = DownloadPath;
            AutoUpdater.InstallationPath = BaseDir;
            AutoUpdater.InstalledVersion = new Version("1.0.0.1");
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.Mandatory = true;
            AutoUpdater.UpdateMode = Mode.Forced;
            AutoUpdater.RunUpdateAsAdmin = true;
            AutoUpdater.Start("http://localhost:5000/AutoUpdater.xml");
        }
    }
}
