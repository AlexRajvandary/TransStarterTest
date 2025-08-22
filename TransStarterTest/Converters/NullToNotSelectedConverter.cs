using System.Globalization;
using System.Windows.Data;

namespace TransStarterTest.Converters
{
    public class NullToNotSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null || (value is string s && s == string.Empty) ? "не выбрано" : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == "не выбрано" ? null : value;
        }
    }
}