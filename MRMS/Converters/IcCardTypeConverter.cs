using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MRMS.Converters
{
    public class IcCardTypeConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals(value, "A"))
            {
                return "管理员卡";
            }
            else if (Equals(value, "T"))
            {
                return "教师卡";
            }
            else if (Equals(value, "S"))
            {
                return "学生卡";
            }
            else if (Equals(value, "L"))
            {
                return "临时卡";
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
