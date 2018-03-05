using System;
using System.Globalization;
using System.Windows.Data;

namespace MRMS.Converters
{
    public class SwitchingConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "无效";
            if ((bool) value)
            {
                return "开";
            }
            else
            {
                return "关";
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
