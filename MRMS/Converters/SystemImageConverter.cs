using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MRMS.Converters
{
    public class SystemImageConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "无效";
            if ((bool)value)
            {
                return string.Format("/Images/Icons/{0}_On.png", parameter);
            }
            else
            {
                return string.Format("/Images/Icons/{0}_Off.png", parameter);
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
