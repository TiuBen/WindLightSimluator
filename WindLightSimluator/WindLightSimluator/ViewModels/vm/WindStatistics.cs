using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;

namespace WindLightSimluator.ViewModels.vm
{
    public class WindStatisticsViewModel : ViewModelBase
    {
        // 存储最近的数据点
        private readonly List<WindViewModel> _history = new List<WindViewModel>();
        private readonly short _currentRunwayDir;
        public WindStatisticsViewModel(short runwayDir)
        {
            _currentRunwayDir = runwayDir;
        }
        // 每当传感器有新数据，调用此方法
        public void AddWindSample(float speed, short dir)
        {
            var newWind = new WindViewModel(speed, dir, _currentRunwayDir);
            _history.Add(newWind);

            // 1. 清理 2 分钟以前的数据
            DateTime threshold = DateTime.Now.AddMinutes(-2);
            _history.RemoveAll(w => w.Timestamp < threshold);

            // 2. 触发所有统计属性的 UI 更新
            UpdateAllStats();
        }

        private void UpdateAllStats()
        {
            // 2分钟属性通知
            OnPropertyChanged(nameof(Avg2WindSpeed));
            OnPropertyChanged(nameof(Avg2WindDir));
            OnPropertyChanged(nameof(Max2WindSpeed));
            OnPropertyChanged(nameof(Min2WindSpeed));
            OnPropertyChanged(nameof(Avg2HeadWindSpeed));
            OnPropertyChanged(nameof(Avg2CrossWindSpeed));

            // 10分钟属性通知 (对应下面的 10 属性)
            OnPropertyChanged(nameof(Avg5WindSpeed));
            OnPropertyChanged(nameof(Avg5WindDir));
            OnPropertyChanged(nameof(Max5WindSpeed));
            OnPropertyChanged(nameof(Min5WindSpeed));
            OnPropertyChanged(nameof(Avg5HeadWindSpeed));
            OnPropertyChanged(nameof(Avg5CrossWindSpeed));
        }

        // --- 统计属性实现 ---
        // ================== 2分钟统计属性 ==================
        public string Avg2WindSpeed => GetAvgSpeed(2);
        public string Avg2WindDir => GetAvgDir(2);
        public string Max2WindSpeed => GetMaxSpeed(2);
        public string Min2WindSpeed => GetMinSpeed(2);
        public string Avg2HeadWindSpeed => GetAvgHeadWind(2);
        public string Avg2CrossWindSpeed => GetAvgCrossWind(2);

        // ================== 10分钟统计属性 ==================
        public string Avg5WindSpeed => GetAvgSpeed(5);
        public string Avg5WindDir => GetAvgDir(5);
        public string Max5WindSpeed => GetMaxSpeed(5);
        public string Min5WindSpeed => GetMinSpeed(5);
        public string Avg5HeadWindSpeed => GetAvgHeadWind(5);
        public string Avg5CrossWindSpeed => GetAvgCrossWind(5);


        // ================== 内部计算逻辑方法 ==================

        private IEnumerable<WindViewModel> GetWindow(int minutes)
            => _history.Where(w => w.Timestamp >= DateTime.Now.AddMinutes(-minutes));

        private string GetAvgSpeed(int min)
        {
            var data = GetWindow(min).ToList();
            return data.Any() ? data.Average(w => w.WindSpeed).ToString("F1") : "0.0";
        }

        private string GetMaxSpeed(int min)
        {
            var data = GetWindow(min).ToList();
            return data.Any() ? data.Max(w => w.WindSpeed).ToString("F1") : "0.0";
        }

        private string GetMinSpeed(int min)
        {
            var data = GetWindow(min).ToList();
            return data.Any() ? data.Min(w => w.WindSpeed).ToString("F1") : "0.0";
        }

        private string GetAvgDir(int min)
        {
            var data = GetWindow(min).ToList();
            if (!data.Any()) return "0";
            // 气象建议：风向平均后取最接近的5度
            double avg = data.Average(w => (double)w.WindDir);
            return ((short)(Math.Round(avg / 5.0) * 5)).ToString();
        }

        private string GetAvgHeadWind(int min)
        {
            var data = GetWindow(min).ToList();
            if (!data.Any()) return "0.0";
            // 解析 WindViewModel 类生成的 HeadWindSpeed 字符串进行平均计算
            return data.Average(w => double.Parse(w.HeadWindSpeed)).ToString("F1");
        }

        private string GetAvgCrossWind(int min)
        {
            var data = GetWindow(min).ToList();
            if (!data.Any()) return "CALM";

            var avg = data.Average(w =>
            {
                string s = w.CrossWindSpeed;
                if (s == "CALM") return 0.0;
                // 提取 L/R 之后的数值部分
                if (double.TryParse(s.Substring(1), out double val))
                {
                    return s.StartsWith("L") ? -val : val;
                }
                return 0.0;
            });

            if (Math.Abs(avg) < 0.1) return "CALM";
            return $"{(avg >= 0 ? "R" : "L")}{Math.Abs(avg):F1}";
        }

    }
}
