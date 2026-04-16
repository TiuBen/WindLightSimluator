using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;


namespace WindLightSimluator.Converters
{
    public class ThresholdColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2) return Brushes.Transparent;

            // 第一个值是输入数字
            double inputNumber = 0;
            if (!double.TryParse(values[0]?.ToString() ?? "0", out inputNumber))
                return Brushes.Transparent;

            // 第二个值是阈值字符串
            string thresholdStr = values[1]?.ToString() ?? "";
            // 第三个值是颜色字符串
            string colorStr = values[2]?.ToString() ?? "";

            if (string.IsNullOrEmpty(thresholdStr) || string.IsNullOrEmpty(colorStr))
                return Brushes.Transparent;

            try
            {
                double[] thresholds = thresholdStr.Split('|')
                    .Select(s => double.Parse(s.Trim()))
                    .OrderBy(x => x)
                    .ToArray();

                string[] colors = colorStr.Split('|')
                    .Select(s => s.Trim())
                    .ToArray();

                if (thresholds.Length != colors.Length)
                    return Brushes.Transparent;

                // 找到匹配的阈值区间
                int index = 0;
                for (int i = 0; i < thresholds.Length; i++)
                {
                    if (inputNumber >= thresholds[i])
                    {
                        index = i;
                    }
                    else
                    {
                        break;
                    }
                }

                if (index >= colors.Length) index = colors.Length - 1;

                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[index]));
            }
            catch
            {
                return Brushes.Transparent;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
