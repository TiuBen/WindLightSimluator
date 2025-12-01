using System;
using System.Globalization;
using System.Windows.Data;


namespace WindLightSimluator.Converters
{
    public class WindSpeedFontSizeConverter:IValueConverter
    {
        public double Scale { get; set; } = 0.8;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double h)
                return h * Scale;

            return 12;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
