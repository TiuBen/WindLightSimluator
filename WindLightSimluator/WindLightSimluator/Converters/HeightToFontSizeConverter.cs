using System;
using System.Globalization;
using System.Windows.Data;

namespace WindLightSimluator.Converters
{
   
    public class HeightToFontSizeConverter : IValueConverter
    {
        public double Scale { get; set; } = 0.8;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double h)
            {
                double scale = Scale;

                if (parameter != null && double.TryParse(parameter.ToString(), out double p))
                    scale = p;

                // 避免 NaN 或 Infinity
                if (double.IsNaN(h) || double.IsInfinity(h))
                    return 12;

                double fontSize = h * scale;

                // 限制最大最小
                if (fontSize < 12) fontSize = 6;
                if (fontSize > 48) fontSize = 60;

                // 四舍五入整数
                return Math.Round(fontSize);
            }

            return 12;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
