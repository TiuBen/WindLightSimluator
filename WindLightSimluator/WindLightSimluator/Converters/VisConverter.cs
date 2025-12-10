using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace WindLightSimluator.Converters
{
  


    public class VisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Brushes.Black;

            if (!int.TryParse(value.ToString(), out int v))
                return GetBrush("VisColor_High");

            if (v < 350)
                return GetBrush("VisColor_Low");

            if (v <= 1500)
                return GetBrush("VisColor_Mid");

            return Brushes.Yellow;
        }


        private Brush GetBrush(string resourceKey)
        {
            // 优先从当前控件资源找，其次 Application 根资源找
            return Application.Current.TryFindResource(resourceKey) as Brush
                   ?? Brushes.Black; // 找不到就给默认值
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
