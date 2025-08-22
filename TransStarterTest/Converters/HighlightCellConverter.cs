using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TransStarterTest.Converters
{
    public class HighlightCellConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3) return Brushes.Transparent;

            if (values[0] is double cellValue &&
                values[1] is double threshold &&
                values[2] is bool highlightEnabled)
            {
                if (!highlightEnabled) return Brushes.Transparent;
                return cellValue >= threshold ? Brushes.Lime : Brushes.Transparent;
            }

            return Brushes.Transparent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

}