using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.Model;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.ViewModels.vm;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace WindLightSimluator.ViewModels.vm
{


    public class WindStatisticsVM : ViewModelBase
    {
        // 存储最近的数据点
        private Queue<Wind> _samples = new();
        private int _maxSize;
        private int _runwayDir;

        public int RunwayDir
        {
            get => _runwayDir;
            set {
                if (value < 0 || value > 360)
                    throw new ArgumentException("跑道方向必须在0-360之间");

                if (SetProperty(ref _runwayDir, value))
                {
                    // ⭐ 跑道变了，必须重新计算
                    UpdateRunwayComponent();
                }
            }
        }

        private bool? _IsActive;
        public bool? IsActive
        {
            get => _IsActive;
            set => SetProperty(ref _IsActive, value);

        }


        public WindStatisticsVM(int runwayDir, int size = 5)
        {
            RunwayDir = runwayDir;
            _maxSize = size;
        }
        // 每当传感器有新数据，调用此方法
        public void AddSample(double speed, double dir)
        {
            _samples.Enqueue(new Wind(speed, dir));

            if (_samples.Count > _maxSize)
                _samples.Dequeue();

            UpdateAll();
        }

        #region 跑道分量统计（平均顶风 / 侧风）

        private double _avgHeadWind;
        public double AvgHeadWind
        {
            get => _avgHeadWind;
            private set => SetProperty(ref _avgHeadWind, value);
        }

        private double _avgCrossWind;
        public double AvgCrossWind
        {
            get => _avgCrossWind;
            private set => SetProperty(ref _avgCrossWind, value);
        }

        #endregion


        #region 风速统计
        private double _minWindSpeed;
        public double MinWindSpeed
        {
            get => _minWindSpeed;
            private set => SetProperty(ref _minWindSpeed, value);
        }
        private double _maxWindSpeed;
        public double MaxWindSpeed
        {
            get => _maxWindSpeed;
            private set => SetProperty(ref _maxWindSpeed, value);
        }
        private double _avgWindSpeed;
        public double AvgWindSpeed
        {
            get => _avgWindSpeed;
            private set => SetProperty(ref _avgWindSpeed, value);
        }

        #endregion

        #region 风向统计
        private double _minWindDir;
        public double MinWindDir
        {
            get => _minWindDir;
            private set => SetProperty(ref _minWindDir, value);
        }
        private double _maxWindDir;
        public double MaxWindDir
        {
            get => _maxWindDir;
            private set => SetProperty(ref _maxWindDir, value);
        }
        private double _avgWindDir;
        public double AvgWindDir
        {
            get => _avgWindDir;
            private set => SetProperty(ref _avgWindDir, value);
        }




        #endregion

        private HashSet<int> _dirRangeSet = new();
        public HashSet<int> DirRangeSet // { get; set; }  // = new HashSet<int> { 0, 1, 2, 3 };
        {
            get => _dirRangeSet;
            set {
                _dirRangeSet = value;
                OnPropertyChanged(nameof(DirRangeSet));
            }
        }

        #region Core Calculation

        private void UpdateAll()
        {
            if (_samples.Count == 0) return;

            UpdateWindSpeed();
            UpdateWindDir();
            UpdateDirection();
            UpdateRunwayComponent();
            UpdateRunwayComponentText();
        }

        #endregion

        #region 计算 Speed 相关的  

        private void UpdateWindSpeed()
        {
            MinWindSpeed = Math.Round(_samples.Min(x => x.WindSpeed), 1);
            MaxWindSpeed = Math.Round(_samples.Max(x => x.WindSpeed), 1);
            AvgWindSpeed = Math.Round(_samples.Average(x => x.WindSpeed), 1);
        }
        private void UpdateWindDir()
        {
            //MinWindDir = Math.Round(_samples.Min(x => x.WindDir), 0);
            //MaxWindDir = Math.Round(_samples.Max(x => x.WindDir), 0);
            //AvgWindDir = Math.Round(_samples.Average(x => x.WindDir), 0);

            //MinWindDir = (int)Math.Round(_samples.Min(x => x.WindDir));
            //MaxWindDir = (int)Math.Round(_samples.Max(x => x.WindDir));
            //AvgWindDir = (int)Math.Round(_samples.Average(x => x.WindDir));

            // Get raw values first
            var minRaw = _samples.Min(x => x.WindDir);
            var maxRaw = _samples.Max(x => x.WindDir);
            var avgRaw = _samples.Average(x => x.WindDir);

            // Handle the 0° = 360° special case
            MinWindDir = NormalizeWindDirection((int)Math.Round(minRaw));
            MaxWindDir = NormalizeWindDirection((int)Math.Round(maxRaw));
            AvgWindDir = NormalizeWindDirection((int)Math.Round(avgRaw));

        }



        #endregion

        #region Direction (矢量平均 + 范围)

        private void UpdateDirection()
        {
            // ===== 矢量平均风向 =====
            double sumSin = 0;
            double sumCos = 0;

            foreach (var s in _samples)
            {
                double rad = s.WindDir * Math.PI / 180.0;
                sumSin += Math.Sin(rad);
                sumCos += Math.Cos(rad);
            }

            double avgRad = Math.Atan2(sumSin, sumCos);
            double avgDeg = avgRad * 180.0 / Math.PI;

            //AvgWindDir = Normalize360(avgDeg);

            // ===== 风向范围 =====
            var indices = _samples
                .Select(x => (int)Math.Round(x.WindDir / 10.0) % 36)
                .OrderBy(x => x)
                .ToList();

            int maxGap = -1;
            int gapStart = 0;
            int gapEnd = 0;

            for (int i = 0; i < indices.Count; i++)
            {
                int current = indices[i];
                int next = indices[(i + 1) % indices.Count];

                int gap = (i == indices.Count - 1)
                    ? (indices[0] + 36 - current)
                    : (next - current);

                if (gap > maxGap)
                {
                    maxGap = gap;
                    gapStart = current;
                    gapEnd = next;
                }
            }

            int start = gapEnd;
            int end = gapStart;

            var set = new HashSet<int>();

            int idx = start;
            while (true)
            {
                set.Add(idx);
                if (idx == end) break;
                idx = (idx + 1) % 36;
            }

            DirRangeSet = set;
        }

        #endregion

        #region Runway Component（平均顶风/侧风）

        // 1. 用于显示的侧风属性 (例如: "R15" 或 "L10")
        private string _avgCrossWindText;
        public string AvgCrossWindText
        {
            get => _avgCrossWindText;
            set => SetProperty(ref _avgCrossWindText, value);
        }

        // 2. 用于显示的顺风/逆风属性 (例如: "+10" 或 "-5")
        private string _avgHeadWindText;
        public string AvgHeadWindText
        {
            get => _avgHeadWindText;
            set => SetProperty(ref _avgHeadWindText, value);
        }

        private void UpdateRunwayComponentText()
        {
            if (_samples == null || _samples.Count == 0)
            {
                AvgHeadWindText = "0.0"; // 或者 "--"
                AvgCrossWindText = "CALM";
                return;
            }

            double sumHead = 0;
            double sumCross = 0;

            foreach (var s in _samples)
            {
                // 1. 计算角度差并归一化
                double delta = NormalizeAngle(s.WindDir - RunwayDir);
                double rad = delta * Math.PI / 180.0;

                // 2. 分解向量
                sumHead += s.WindSpeed * Math.Cos(rad);
                sumCross += s.WindSpeed * Math.Sin(rad);
            }

            // 3. 计算平均值
            double avgHeadVal = sumHead / _samples.Count;
            double avgCrossVal = sumCross / _samples.Count;

            // 4. 格式化 Head Wind (逆风/顺风)
            // 逻辑：正数是逆风 (+)，负数是顺风 (-)
            double headAbs = Math.Round(Math.Abs(avgHeadVal), 1);
            AvgHeadWindText = avgHeadVal >= 0
                ? $"+{headAbs}"   // 逆风
                : $"-{headAbs}";  // 顺风

            // 5. 格式化 Cross Wind (左侧风/右侧风)
            // 逻辑：正数是右侧风 (R)，负数是左侧风 (L)
            double crossAbs = Math.Round(Math.Abs(avgCrossVal), 1);

            if (crossAbs < 0.1) // 视为无风
            {
                AvgCrossWindText = "CALM";
            }
            else
            {
                // 根据你的需求，这里生成 "R15" 或 "L10" 这种格式
                string side = avgCrossVal > 0 ? "R" : "L";
                AvgCrossWindText = $"{side}{crossAbs}";
            }
        }

        private void UpdateRunwayComponent()
        {
            double sumHead = 0;
            double sumCross = 0;

            foreach (var s in _samples)
            {
                double delta = Normalize180(s.WindDir - RunwayDir);
                double rad = delta * Math.PI / 180.0;

                sumHead += s.WindSpeed * Math.Cos(rad);
                sumCross += s.WindSpeed * Math.Sin(rad);
            }

            AvgHeadWind = Math.Round(sumHead / _samples.Count, 1);
            AvgCrossWind = Math.Round(sumCross / _samples.Count, 1);
        }

        #endregion

        #region Helpers

        private static double Normalize360(double angle)
        {
            angle %= 360;
            if (angle < 0) angle += 360;
            return angle;
        }

        private static double Normalize180(double angle)
        {
            angle = Normalize360(angle);
            if (angle > 180) angle -= 360;
            return angle;
        }

        private int NormalizeWindDirection(int direction)
        {
            // If direction is 0, convert to 360
            if (direction == 0)
                return 360;

            // Ensure direction is a multiple of 10 (rounding to nearest 10 if needed)
            direction = (int)Math.Round(direction / 10.0) * 10;

            // Handle case where rounding gives 0 (e.g., 5° rounds to 0°)
            if (direction == 0)
                return 360;

            // Ensure direction stays within 10-360 range
            if (direction < 10)
                direction = 10;
            else if (direction > 360)
                direction = 360;

            return direction;
        }

        private static double NormalizeAngle(double angle)
        {
            if (angle < 0) return 0.0;
            angle %= 360;
            if (angle > 180) angle -= 360;
            if (angle < -180) angle += 360;
            return angle;
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