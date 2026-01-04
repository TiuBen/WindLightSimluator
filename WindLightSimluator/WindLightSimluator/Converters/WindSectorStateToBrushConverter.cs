using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using System.Diagnostics;
using System.Windows.Automation.Provider;

namespace WindLightSimluator.Converters
{
    public class WindSectorStateToBrushConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type t, object p, CultureInfo c)
        {
            if (values.Length < 3)
                return Brushes.Yellow;

            bool isActive = values[0] is bool b && b;
            int arcIndex = -1;
            if (values[1] != null)
            {
                if (values[1] is int i)
                    arcIndex = i;
                else if (!int.TryParse(values[1].ToString(), out arcIndex))
                    arcIndex = -1;
            }
            var rangeArcIndex = values[2] as IEnumerable<int>;
            int directArcIndex = values[3] is int d ? d : -1;

            //
            bool isDirect = arcIndex == directArcIndex;
            bool isInRange = rangeArcIndex != null && rangeArcIndex.Contains(arcIndex);

            string resultKey;

            // ---------- 1️⃣ 正向风 ----------
            if (isDirect)
            {
                resultKey = isActive
                    ? "WindPanelArcDirectedHeadColor.Active"
                    : "WindPanelArcDirectedHeadColor.InActive";
            }
            // ---------- 2️⃣ 风向范围 ----------
            else if (isInRange)
            {
                resultKey = isActive
                    ? "WindPanelArcRangeColor.Active"
                    : "WindPanelArcRangeColor.InActive";
            }
            // ---------- 3️⃣ 其他 ----------
            else
            {
                resultKey = "Default(Blue)";
            }

            //Debug.WriteLine(
            //    $"[WindArc] arc={arcIndex:00} | " +
            //    $"Active={isActive} | " +
            //    $"Direct={directArcIndex:00} | " +
            //    $"IsDirect={isDirect} | " +
            //    $"InRange={isInRange} | " +
            //    $"Brush={resultKey}"
            //);


            // ---------- 1️⃣ 正向风（最高优先级） ----------
            if (arcIndex == directArcIndex)
            {
                return Application.Current.FindResource(
                    isActive
                        ? "WindPanelArcDirectedHeadColor.Active"
                        : "WindPanelArcDirectedHeadColor.InActive"
                );
            }

            // ---------- 2️⃣ 风向范围 ----------
            if (rangeArcIndex != null && rangeArcIndex.Contains(arcIndex))
            {
                return Application.Current.FindResource(
                    isActive
                        ? "WindPanelArcRangeColor.Active"
                        : "WindPanelArcRangeColor.InActive"
                );
            }

            // ---------- 3️⃣ 其他 ----------
            return Application.Current.FindResource(
                    isActive
                        ? "WindPanelArcColor.Active"
                        : "WindPanelArcColor.InActive"
                );
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
