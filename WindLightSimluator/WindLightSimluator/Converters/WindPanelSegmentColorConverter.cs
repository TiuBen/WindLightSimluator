using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (values[1] is not IEnumerable<int> range)
                return WindSegmentState.Error;

            // IsActive
            bool isActive = false;
            if (values[2] is bool b)
                isActive = b;

            // AngleIndex
            if (values[3] is not int angleIndex)
                return WindSegmentState.Error;
            //Debug.WriteLine($"AngleIndex={angleIndex}");

            // 🎯 JustAhead（优先级最高）
            if (tag == angleIndex)
            {
                //Debug.WriteLine($" tag=={tag} just angleIndex{angleIndex} ");
                return isActive
                    ? WindSegmentState.JustAheadActive
                    : WindSegmentState.JustAheadInactive;
            }

            // 🎯 InRange / OutRange
            bool inRange = range.Contains(tag);

            if (inRange)
            {
                //Debug.WriteLine($"tag =={tag} inRange");
                return isActive
                    ? WindSegmentState.InRangeActive
                    : WindSegmentState.InRangeInactive;
            }
            else
            {
                //Debug.WriteLine($"tag=={tag} not  inRange");
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
