using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WindLightSimluator.Converters
{
    // 通用枚举转布尔转换器
    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null || parameter == null)
                return false;

            // 比较枚举值和参数是否相等

            return value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter != null)
            {
                // 将参数转换回枚举类型
                return Enum.Parse(targetType, parameter.ToString());
            }
            return Binding.DoNothing;
        }
    }
}
