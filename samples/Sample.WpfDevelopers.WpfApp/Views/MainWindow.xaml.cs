using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows.Controls;
using WPFDevelopers.Controls;

namespace Sample.WpfDevelopers.WpfApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AudioWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawerMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myFrame == null)
                return;
            var menu = (WPFDevelopers.Controls.DrawerMenu)sender;

            if (menu == null)
                return;
            var item = (WPFDevelopers.Controls.DrawerMenuItem)menu.SelectedValue;
            if (item == null)
                return;

            if (item.Tag == null)
                return;
            var url = item.Tag.ToString();
            if (string.IsNullOrEmpty(url))
                return;

            var content = Ioc.Default.GetView(url);
            if (content is null)
                return;
            myFrame.Navigate(content);
        }
    }
}
