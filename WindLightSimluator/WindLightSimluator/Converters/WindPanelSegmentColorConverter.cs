using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WindLightSimluator.Converters
{
    public class WindPanelSegmentColorConverter : IMultiValueConverter
    {
        public Brush ActiveBrush { get; set; } = Brushes.Green;
        public Brush InactiveBrush { get; set; } = Brushes.Gray;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return Brushes.Gray;

            if (values[0] is not string tagStr || !int.TryParse(tagStr, out int tag))
                return Brushes.Yellow;

            if (values[1] is not HashSet<int> set)
                return Brushes.Green;

            return set.Contains(tag) ? Brushes.Red : Brushes.Gray;


        }




        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
