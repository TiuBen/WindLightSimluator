using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WindLightSimluator.ViewModels;

namespace WindLightSimluator.Converters
{
    public class RunwayPartVisibiltyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Return Collapsed by default (not visible)
            if (value == null || parameter == null)
                return Visibility.Collapsed;

            // Get the current part type from the ViewModel
            if (value is not RunwayPartType currentPart)
                return Visibility.Collapsed;

            // Get the target part type from the converter parameter
            if (!Enum.TryParse<RunwayPartType>(parameter.ToString(), true, out var targetPart))
                return Visibility.Collapsed;

            // Return Visible if the current part matches the target part
            return currentPart == targetPart ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
