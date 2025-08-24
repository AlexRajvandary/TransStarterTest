using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace TransStarterTest.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[]?)fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes?.FirstOrDefault()?.Description ?? value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var field in targetType.GetFields())
            {
                var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                if ((attr != null && attr.Description == (string)value) || field.Name == (string)value)
                    return Enum.Parse(targetType, field.Name);
            }
            return Enum.Parse(targetType, value.ToString());
        }
    }
}
