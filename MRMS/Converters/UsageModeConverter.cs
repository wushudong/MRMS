using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MRMS.Converters
{
    public class UsageModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals(value, "1"))
            {
                return "公共刷卡";
            }
            else if(Equals(value, "2"))
            {
                return "指定刷卡";
            }
            else if (Equals(value, "3"))
            {
                return "管理模式";
            }
            else if (Equals(value, "9"))
            {
                return "试运行模式";
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
