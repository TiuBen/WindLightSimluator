using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WindLightSimluator.Converters
{
    internal class ToggleValueConverter : IMultiValueConverter
    {
        // values:
        // [0] = IsOn
        // [1] = OnValue
        // [2] = OffValue
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3) return false;

            var current = values[0];
            var onValue = values[1];

            return Equals(current, onValue);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;

            // parameter 传入 OnValue / OffValue
            if (parameter is object[] param && param.Length == 2)
            {
                return new object[]
                {
                isChecked ? param[0] : param[1], // IsOn
                Binding.DoNothing,
                Binding.DoNothing
                };
            }

            return new object[] { Binding.DoNothing };
        }
    }
}
