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
        public Brush ActiveBrush { get; set; }
        public Brush InactiveBrush { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return InactiveBrush;

            if (values[0] is not string tagStr || !int.TryParse(tagStr, out int tag))
                return InactiveBrush;

            if (values[1] is not HashSet<int> set)
                return InactiveBrush;

            return set.Contains(tag) ? ActiveBrush : InactiveBrush;
        }


      

       public  object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
