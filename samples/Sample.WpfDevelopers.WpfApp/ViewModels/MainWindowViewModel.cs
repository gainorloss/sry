using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FontAwesome.Sharp;
using Gs.LOL;
using Microsoft.Expression.Drawing.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFDevelopers.Controls;

namespace Sample.WpfDevelopers.WpfApp.ViewModels
{
    internal partial class MainWindowViewModel : ObservableObject
    {
        private readonly LeagueClient _league;

        [ObservableProperty]
        private bool _connected = false;

        public ObservableCollection<DrawerMenuItem> MenuItems { get; set; }
        public MainWindowViewModel(LeagueClient league)
        {
            LoadMenus();
            _league = league;
        }

        [RelayCommand]
        public async Task ClientConnectAsync()
        {
            Connected = true;
        }

        [RelayCommand]
        public void Quit()
        {
            Application.Current.Shutdown();
        }

        [RelayCommand]
        public void About()
        {
        }

        [RelayCommand]
        public void Twink()
        {
        }

        [RelayCommand]
        public void SendMessage()
        {
            NotifyIcon.ShowBalloonTip("Message", " Welcome to WPFDevelopers.Minimal ", NotifyIconInfoType.None);
        }

        [RelayCommand]
        public void DrawerMenuItemSelect()
        {

        }

        private void LoadMenus()
        {
            var menuPath = Path.Combine(AppContext.BaseDirectory, "menu.json");
            var json = File.ReadAllText(menuPath);
            var menus = JsonSerializer.Deserialize<IEnumerable<MenuItem>>(json);
            MenuItems = new ObservableCollection<DrawerMenuItem>();
            MenuItems.AddRange(menus.Select(i => new DrawerMenuItem
            {
                Text = i.Title,
                Icon = new IconImage() { Icon = (IconChar)Enum.Parse(typeof(IconChar), i.Icon), Foreground = System.Windows.Media.Brushes.DarkBlue },
                Tag = i.Url,
                IsEnabled = string.IsNullOrEmpty(i.Url) ? false : !i.IsDisabled
            }));
            MenuItems.Add(new DrawerMenuItem
            {
                Text = "测试",
                Icon = new IconImage() { Icon = (IconChar)Enum.Parse(typeof(IconChar), "Dev"), Foreground = System.Windows.Media.Brushes.Orange },
                Tag = "TestPage"
            });

            #region Obsolete.
            //MenuItems = new ObservableCollection<DrawerMenuItem>
            //{
            //     new DrawerMenuItem{Text="主页",Icon=new IconImage(){  Icon=(IconChar)Enum.Parse(typeof(IconChar),"Home")} ,Tag="HomePage"},
            //     new DrawerMenuItem{Text="Edge",Icon=new IconImage(){  Icon=IconChar.Edge} ,Tag="EdgePage" },
            //     new DrawerMenuItem{Text="云盘",Icon=new IconImage(){  Icon=IconChar.PlateWheat},IsEnabled=false},
            //     new DrawerMenuItem{Text="邮件",Icon=new IconImage(){  Icon=IconChar.MailBulk} },
            //     new DrawerMenuItem{Text="视频",Icon=new IconImage(){  Icon=IconChar.Video },Tag="Sample.WpfDevelopers.Mc;HomePage" },
            //     new DrawerMenuItem{Text="Bus",Icon=new IconImage(){  Icon=IconChar.Bus} }
            //}; 
            #endregion
        }
    }


    public class MenuItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string Color { get; set; }
        public bool IsDisabled { get; set; }
    }
}

