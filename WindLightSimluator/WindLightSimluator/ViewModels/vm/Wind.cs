using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
    public class WindVM : ViewModelBase
    {
        private double _windSpeed;
        private double   _windDir;
        private int _runwayDir;
        public WindVM(double speed, double dir, int rwyDir)
        {
            _runwayDir = rwyDir;

            WindSpeed = speed;
            WindDir = dir;
            Timestamp = DateTime.Now;
        }


        public DateTime Timestamp { get; private set; }

        public double WindSpeed
        {
            get => Math.Round(_windSpeed, 1);
            set {
                if (value < 0) throw new ArgumentException("风速不能为负数");
                if (SetProperty(ref _windSpeed, value))
                {
                    // 当风速改变，必须通知 UI 重新读取计算属性
                    OnPropertyChanged(nameof(HeadWindSpeed));
                    OnPropertyChanged(nameof(CrossWindSpeed));

                }
            }
        }

        public double WindDir
        {
            get => (short)(Math.Round(_windDir / 10.0) * 10);
            set {
                if (value < 0 || value > 360) throw new ArgumentException("风向必须在0-360度之间");
                if (SetProperty(ref _windDir, value))
                {
                    OnPropertyChanged(nameof(HeadWindSpeed));
                    OnPropertyChanged(nameof(CrossWindSpeed));
                    OnPropertyChanged(nameof(AngleIndex));
                }
            }
        }

        // --- 新增：角度索引 (0-35) ---
        public int AngleIndex
        {
            //get {
            //    double roundedDir = Math.Round(_windDir / 10.0) * 10;
            //    int index = (int)((roundedDir + 5) / 10);
            //    return index >= 36 ? 0 : index;
            //}

            get {
                    double dir = _windDir;

                    // 关键：平移5度做区间归属
                    dir = (dir + 5) % 360;

                    return (int)(dir / 10);
            }
        }

        public double AngleHeading
        {
            get {
                double roundedDir = Math.Round(_windDir / 10.0) * 10;
                int index = (int)((roundedDir + 5) / 10); if (index >= 36) index = 0;
                return index * 10;
            }
        }

        


        private bool? _isactive;
        public bool? IsActive
        {
            get => _isactive;
            set => SetProperty(ref _isactive, value);

        }




        // ... 保留你原有的 NormalizeAngle, HeadWindSpeed, CrossWindSpeed 逻辑 ...
        // 注意：HeadWindSpeed 和 CrossWindSpeed 只有 get，不需要 SetProperty，
        // 只要在 WindSpeed/Dir 的 setter 里调用 OnPropertyChanged(nameof(HeadWindSpeed)) 即可。

        private static double NormalizeAngle(double angle)
        {
            if (angle < 0) return 0.0;
            angle %= 360;
            if (angle > 180) angle -= 360;
            if (angle < -180) angle += 360;
            return angle;
        }


        public string HeadWindSpeed
        {
            get {
                double delta = NormalizeAngle(WindDir - _runwayDir);
                double hw = WindSpeed * Math.Cos(delta * Math.PI / 180.0);

                double value = Math.Round(Math.Abs(hw), 1);

                return hw >= 0
                    ? $"+{value}"   // 逆风
                    : $"-{value}";  // 顺风
            }
        }

        // 计算侧风分量（需要传入跑道方向）
        public string CrossWindSpeed
        {
            get {
                double delta = NormalizeAngle(WindDir - _runwayDir);
                double cw = WindSpeed * Math.Sin(delta * Math.PI / 180.0);

                double value = Math.Round(Math.Abs(cw), 1);

                if (value == 0)
                    return "CALM";

                string side = cw > 0 ? "R" : "L";

                return $"{side}{value}";
            }

        }

    }
}
