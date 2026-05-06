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
    public enum WindSegmentState
    {
        InRangeActive,
        InRangeInactive,
        OutRangeActive,
        OutRangeInactive,
        JustAheadActive,
        JustAheadInactive,
        Error
    }

    public class WindPanelSegmentColorConverter : IMultiValueConverter
    {
   
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // 0 Tag（当前扇区索引）
            // 1 DirRangeSet
            // 2 IsActive
            // 3 AngleIndex（当前风向）

            if (values == null || values.Length != 4)
                return WindSegmentState.Error;

            // Tag
            if (values[0] is not string tagStr || !int.TryParse(tagStr, out int tag))
                return WindSegmentState.Error;

            // Range
            if (values[1] is not IEnumerable<int> set)
                return WindSegmentState.Error;

            // IsActive
            if (values[2] is not bool isActive)
                return WindSegmentState.Error;

            // AngleIndex
            if (values[3] is not int angleIndex)
                return WindSegmentState.Error;

            // 🎯 JustAhead（优先级最高）
            if (tag == angleIndex)
            {
                return isActive
                    ? WindSegmentState.JustAheadActive
                    : WindSegmentState.JustAheadInactive;
            }

            // 🎯 InRange / OutRange
            bool inRange = set.Contains(tag);

            if (inRange)
            {
                return isActive
                    ? WindSegmentState.InRangeActive
                    : WindSegmentState.InRangeInactive;
            }
            else
            {
                return isActive
                    ? WindSegmentState.OutRangeActive
                    : WindSegmentState.OutRangeInactive;
            }

        }




        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
