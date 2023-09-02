using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Sample.WpfDevelopers.WpfApp.Converters
{
    internal class ConnectedToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool temp = (bool)value;
            return temp ? "已连接" : "未连接";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
