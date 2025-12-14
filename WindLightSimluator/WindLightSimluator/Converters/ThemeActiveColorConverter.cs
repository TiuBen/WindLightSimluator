using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Diagnostics;


namespace WindLightSimluator.Converters
{
    public class ThemeActiveColorConverter : IMultiValueConverter
    {
        // parameter 可以传 ResourceKey 前缀，例如 "TextBrush"
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine("parameter:"+parameter);

            if (values.Length < 2) return Brushes.Red;

            bool isActive = values[0] is bool b && b;
            Debug.WriteLine("isActive:" + isActive);

            string theme = values[1] as string ?? "Light";
            Debug.WriteLine("theme:" + theme);


            // parameter = "Rvr.Background" / "Rvr.Text"
            if (parameter is not string prefix)
            {
                Debug.WriteLine("parameter is not string prefix");
                return Brushes.Magenta;

            }



            //主题+元素para+状态
            //string colorKey = $"{theme}.{parameter}.{(isActive ? "Active" : "Inactive")}";
            //string colorKey = $"{theme}.{parameter}.{(isActive ? "Active" : "Inactive")}";
            string colorKey = $"{parameter}.{(isActive ? "Active" : "Inactive")}";
            //string colorKey = "LabelBgActive";

            Debug.WriteLine("colorKey:"+colorKey);

            //theme + "_" + (isActive ?  : "InactiveBrush");

            // TryFindResource 从最外层逻辑树查找资源
            if (Application.Current.TryFindResource(colorKey) is Brush brush)
                return brush;

            return Brushes.Magenta;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}

