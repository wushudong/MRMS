using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MRMS.Converters
{
    public class VideoPlayControlVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((Meta.Vlc.Interop.Media.MediaState) value == Meta.Vlc.Interop.Media.MediaState.Playing)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
