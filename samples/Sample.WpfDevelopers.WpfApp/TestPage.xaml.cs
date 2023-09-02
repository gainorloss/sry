using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Sample.WpfDevelopers.WpfApp
{
    /// <summary>
    /// TestPage.xaml 的交互逻辑
    /// </summary>
    public partial class TestPage : Page
    {

        public TestPage()
        {
            InitializeComponent();
           
        }
    }
}

public class UserModel
{
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}
