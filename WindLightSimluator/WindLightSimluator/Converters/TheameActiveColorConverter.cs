using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WindLightSimluator.Converters
{
    public class TheameActiveColorConverter:IMultiValueConverter
    {
        // parameter 可以传 ResourceKey 前缀，例如 "TextBrush"
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return Brushes.Red;

            bool isActive = values[0] is bool b && b;
            string theme = values[1] as string ?? "Day";
            string resourcePrefix = parameter as string ?? "TextBrush";

            string key = theme + "_" + (isActive ? resourcePrefix : "InactiveBrush");

            // TryFindResource 从最外层逻辑树查找资源
            if (Application.Current.TryFindResource(key) is Brush brush)
                return brush;

            return Brushes.Black;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}

