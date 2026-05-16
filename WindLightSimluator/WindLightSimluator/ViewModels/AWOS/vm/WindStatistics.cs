using System;
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

        private bool? _IsActive;
        public bool? IsActive
        {
            get => _IsActive;
            set => SetProperty(ref _IsActive, value);
        }

        private RunwayPartVM? _parent;

        public RunwayPartVM? BelongPart
        {
            get => _parent;
            set {
                // 取消旧订阅
                if (_parent != null)
                {
                    _parent.PropertyChanged -= Parent_PropertyChanged;
                }

                _parent = value;

                // 监听新父对象
                if (_parent != null)
                {
                    _parent.PropertyChanged += Parent_PropertyChanged;
                }

                // 刷新顶风/侧风
                OnPropertyChanged(nameof(HeadWindSpeedAvg2));
                OnPropertyChanged(nameof(CrossWindSpeedAvg2));
            }
        }
        private double RunwayDirValue => BelongPart?.PartDirection ?? -1;


        /// <summary>
        /// 有效时间（分钟）
        /// </summary>
        public int Duration { get; set; } = 2;

        /// <summary>
        /// 当前系统时间
        /// </summary>
        public DateTime CurrentTime => DateTime.Now;

        /// <summary>
        /// 所有风数据
        /// </summary>
        private readonly Queue<WindVM> _allData = new();




        /// <summary>
        /// 添加风数据
        /// </summary>
        public void AddWindVM(WindVM wind)
        {

            DateTime limit = DateTime.Now.AddMinutes(-Duration);

            while (_allData.Count > 0 &&
               _allData.Peek().Timestamp < limit)
            {
                _allData.Dequeue();
            }

            // 添加新数据
            _allData.Enqueue(wind);

            // 更新方向区间集合
            UpdateDirRangeSets();

            // 通知刷新
            OnPropertyChanged(nameof(WindDirMin2));
            OnPropertyChanged(nameof(WindDirAvg2));
            OnPropertyChanged(nameof(WindDirMax2));

            OnPropertyChanged(nameof(WindSpeedMin2));
            OnPropertyChanged(nameof(WindSpeedAvg2));
            OnPropertyChanged(nameof(WindSpeedMax2));

            OnPropertyChanged(nameof(HeadWindSpeedAvg2));
            OnPropertyChanged(nameof(CrossWindSpeedAvg2));

            OnPropertyChanged(nameof(OutDirRangeSet));
            OnPropertyChanged(nameof(InnerRangeSet));

        }

        #region 风向统计

        /// <summary>
        /// 最小风向（显示值：10度取整）
        /// </summary>
        public int WindDirMin2
        {
            get {
                if (_allData.Count == 0)
                    return 999;

                double dir = _allData.Min(x => x.WindDirValue);

                int result = (int)(Math.Round(dir / 10.0) * 10);

                return result == 0 ? 360 : result;
            }
        }

        /// <summary>
        /// 最大风向（显示值：10度取整）
        /// </summary>
        public int WindDirMax2
        {
            get {
                if (_allData.Count == 0)
                    return 999;

                double dir = _allData.Max(x => x.WindDirValue);

                int result = (int)(Math.Round(dir / 10.0) * 10);

                return result == 0 ? 360 : result;
            }
        }

        /// <summary>
        /// 平均风向（向量平均 + 10度取整）
        /// </summary>
        public int WindDirAvg2
        {
            get {
                if (_allData.Count == 0)
                    return 999;

                double sinSum = _allData.Sum(x =>
                    Math.Sin(x.WindDirValue * Math.PI / 180.0));

                double cosSum = _allData.Sum(x =>
                    Math.Cos(x.WindDirValue * Math.PI / 180.0));

                double avgRad = Math.Atan2(sinSum, cosSum);

                double avgDeg = avgRad * 180.0 / Math.PI;

                if (avgDeg < 0)
                    avgDeg += 360;

                int result = (int)(Math.Round(avgDeg / 10.0) * 10);

                return result == 0 ? 360 : result;
            }
        }

        #endregion

        #region 风速统计

        public string WindSpeedMin2
        {
            get {
                if (_allData.Count == 0)
                    return "CALM";

                double value =
                    Math.Round(
                        _allData.Min(x => x.WindSpeedValue),
                        1);

                return value == 0
                    ? "CALM"
                    : value.ToString("0.0");
            }
        }

        public string WindSpeedAvg2
        {
            get {
                if (_allData.Count == 0)
                    return "CALM";

                double value =
                    Math.Round(
                        _allData.Average(x => x.WindSpeedValue),
                        1);

                return value == 0
                    ? "CALM"
                    : value.ToString("0.#");
            }
        }

        public string WindSpeedMax2
        {
            get {
                if (_allData.Count == 0)
                    return "CALM";

                double value =
                    Math.Round(
                        _allData.Max(x => x.WindSpeedValue),
                        1);

                return value == 0
                    ? "CALM"
                    : value.ToString("0.#");
            }
        }

        #endregion

        #region 顶风 / 侧风

        public string HeadWindSpeedAvg2
        {
            get {
                if (_allData.Count == 0)
                    return "CALM";

                if (RunwayDirValue < 0)
                    return "ERROR";

                double hw =
                    _allData.Average(x =>
                    {
                        double delta =
                            x.WindDirValue
                            - RunwayDirValue;

                        return x.WindSpeedValue *
                               Math.Cos(
                                   delta
                                   * Math.PI / 180.0);
                    });

                double value =
                    Math.Round(Math.Abs(hw), 1);

                if (value == 0)
                    return "CALM";

                return hw >= 0
                    ? value.ToString("0.#")
                    : $"-{value:0.#}";
            }
        }

        public string CrossWindSpeedAvg2
        {
            get {
                if (_allData.Count == 0)
                    return "CALM";

                if (RunwayDirValue < 0)
                    return "ERROR";

                double cw =
                    _allData.Average(x =>
                    {
                        double delta =
                            x.WindDirValue
                            - RunwayDirValue;

                        return x.WindSpeedValue *
                               Math.Sin(
                                   delta
                                   * Math.PI / 180.0);
                    });

                double value =
                    Math.Round(Math.Abs(cw), 1);

                if (value == 0)
                    return "CALM";

                string side =
                    cw > 0 ? "R" : "L";

                return $"{side}{value:0.#}";
            }
        }

        #endregion



        private void Parent_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RunwayPartVM.PartDirection))
            {
                OnPropertyChanged(nameof(HeadWindSpeedAvg2));
                OnPropertyChanged(nameof(CrossWindSpeedAvg2));
            }
        }
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