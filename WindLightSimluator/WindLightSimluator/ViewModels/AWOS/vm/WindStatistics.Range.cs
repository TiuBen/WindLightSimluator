using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.Model;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.ViewModels.vm;

namespace WindLightSimluator.ViewModels.vm
{

    public partial class WindStatisticsVM : ViewModelBase
    {
        #region Core Calculation

        private readonly HashSet<int> _outerDirRangeSet = new();

        public HashSet<int> OutDirRangeSet => _outerDirRangeSet;

        private readonly HashSet<int> _innerDirRangeSet = new();

        public HashSet<int> InnerRangeSet => _innerDirRangeSet;

        /// <summary>
        /// 风向转 0~35 扇区
        /// 355~5 => 0
        /// 5~15 => 1
        /// ...
        /// </summary>
        private int GetDirRange(double dir)
        {
            return (int)Math.Floor((dir + 5) / 10.0) % 36;
        }

        /// <summary>
        /// 根据一组风向
        /// 计算最短包络范围
        /// </summary>
        private HashSet<int> BuildDirRangeSet(IEnumerable<WindVM> winds)
        {
            HashSet<int> result = new();

            var dirs = winds
                .Select(x => x.WindDirValue)
                .OrderBy(x => x)
                .ToList();

            if (dirs.Count == 0)
                return result;

            // 只有一个点
            if (dirs.Count == 1)
            {
                result.Add(GetDirRange(dirs[0]));
                return result;
            }

            // =====================================
            // 找最大缺口
            // =====================================

            double maxGap = -1;

            int maxGapIndex = 0;

            for (int i = 0; i < dirs.Count; i++)
            {
                double current = dirs[i];

                double next =
                    (i == dirs.Count - 1)
                    ? dirs[0] + 360
                    : dirs[i + 1];

                double gap = next - current;

                if (gap > maxGap)
                {
                    maxGap = gap;
                    maxGapIndex = i;
                }
            }

            // =====================================
            // 最短包络范围
            // =====================================

            double startDir = dirs[(maxGapIndex + 1) % dirs.Count];

            double endDir = dirs[maxGapIndex];

            int startSector = GetDirRange(startDir);

            int endSector = GetDirRange(endDir);

            // =====================================
            // 填充范围
            // =====================================

            int sector = startSector;

            while (true)
            {
                result.Add(sector);

                if (sector == endSector)
                    break;

                sector = (sector + 1) % 36;
            }

            return result;
        }

        /// <summary>
        /// 更新风向范围集合
        /// </summary>
        private void UpdateDirRangeSets()
        {
            _outerDirRangeSet.Clear();
            _innerDirRangeSet.Clear();

            // 最近10条
            foreach (int dir in BuildDirRangeSet(
                _allData.TakeLast(10)))
            {
                _outerDirRangeSet.Add(dir);
            }

            // 最近30条
            foreach (int dir in BuildDirRangeSet(
                _allData.TakeLast(25)))
            {
                _innerDirRangeSet.Add(dir);
            }
        }

        #endregion




    }
}




//        private void UpdateAllStats()
//{
//    // 2分钟属性通知
//    OnPropertyChanged(nameof(Avg2WindSpeed));
//    OnPropertyChanged(nameof(Avg2WindDir));
//    OnPropertyChanged(nameof(Max2WindSpeed));
//    OnPropertyChanged(nameof(Min2WindSpeed));
//    OnPropertyChanged(nameof(Avg2HeadWindSpeed));
//    OnPropertyChanged(nameof(Avg2CrossWindSpeed));

//    // 10分钟属性通知 (对应下面的 10 属性)
//    OnPropertyChanged(nameof(Avg5WindSpeed));
//    OnPropertyChanged(nameof(Avg5WindDir));
//    OnPropertyChanged(nameof(Max5WindSpeed));
//    OnPropertyChanged(nameof(Min5WindSpeed));
//    OnPropertyChanged(nameof(Avg5HeadWindSpeed));
//    OnPropertyChanged(nameof(Avg5CrossWindSpeed));
//}

//// --- 统计属性实现 ---
//// ================== 2分钟统计属性 ==================
//public string Avg2WindSpeed => GetAvgSpeed(2);
//public string Avg2WindDir => GetAvgDir(2);
//public string Max2WindSpeed => GetMaxSpeed(2);
//public string Min2WindSpeed => GetMinSpeed(2);
//public string Avg2HeadWindSpeed => GetAvgHeadWind(2);
//public string Avg2CrossWindSpeed => GetAvgCrossWind(2);

//// ================== 10分钟统计属性 ==================
//public string Avg5WindSpeed => GetAvgSpeed(5);
//public string Avg5WindDir => GetAvgDir(5);
//public string Max5WindSpeed => GetMaxSpeed(5);
//public string Min5WindSpeed => GetMinSpeed(5);
//public string Avg5HeadWindSpeed => GetAvgHeadWind(5);
//public string Avg5CrossWindSpeed => GetAvgCrossWind(5);


//// ================== 内部计算逻辑方法 ==================

//private IEnumerable<WindVM> GetWindow(int minutes)
//    => _history.Where(w => w.Timestamp >= DateTime.Now.AddMinutes(-minutes));

//private string GetAvgSpeed(int min)
//{
//    var data = GetWindow(min).ToList();
//    return data.Any() ? data.Average(w => w.WindSpeed).ToString("F1") : "0.0";
//}

//private string GetMaxSpeed(int min)
//{
//    var data = GetWindow(min).ToList();
//    return data.Any() ? data.Max(w => w.WindSpeed).ToString("F1") : "0.0";
//}

//private string GetMinSpeed(int min)
//{
//    var data = GetWindow(min).ToList();
//    return data.Any() ? data.Min(w => w.WindSpeed).ToString("F1") : "0.0";
//}

//private string GetAvgDir(int min)
//{
//    var data = GetWindow(min).ToList();
//    if (!data.Any()) return "0";
//    // 气象建议：风向平均后取最接近的5度
//    double avg = data.Average(w => (double)w.WindDir);
//    return ((short)(Math.Round(avg / 5.0) * 5)).ToString();
//}

//private string GetAvgHeadWind(int min)
//{
//    var data = GetWindow(min).ToList();
//    if (!data.Any()) return "0.0";
//    // 解析 WindVM 类生成的 HeadWindSpeed 字符串进行平均计算
//    return data.Average(w => double.Parse(w.HeadWindSpeed)).ToString("F1");
//}

//private string GetAvgCrossWind(int min)
//{
//    var data = GetWindow(min).ToList();
//    if (!data.Any()) return "CALM";

//    var avg = data.Average(w =>
//    {
//        string s = w.CrossWindSpeed;
//        if (s == "CALM") return 0.0;
//        // 提取 L/R 之后的数值部分
//        if (double.TryParse(s.Substring(1), out double val))
//        {
//            return s.StartsWith("L") ? -val : val;
//        }
//        return 0.0;
//    });

//    if (Math.Abs(avg) < 0.1) return "CALM";
//    return $"{(avg >= 0 ? "R" : "L")}{Math.Abs(avg):F1}";
//}