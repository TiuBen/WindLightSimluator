using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
    public class WindViewModel : ViewModelBase
    {
        private double _windSpeed;
        private short _windDir;
        private int _runwayDir;
        // 记录时间，用于后续统计过滤
        public DateTime Timestamp { get; private set; }

        public double WindSpeed
        {
            get => (float)Math.Round(_windSpeed, 1);
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

        public short WindDir
        {
            get => (short)(Math.Round(_windDir / 5.0) * 5);
            set {
                if (value < 0 || value > 360) throw new ArgumentException("风向必须在0-360度之间");
                if (SetProperty(ref _windDir, value))
                {
                    OnPropertyChanged(nameof(HeadWindSpeed));
                    OnPropertyChanged(nameof(CrossWindSpeed));
                }
            }
        }

        public WindViewModel(double speed, short dir, int rwyDir)
        {
            _runwayDir = rwyDir;
            WindSpeed = speed;
            WindDir = dir;
            Timestamp = DateTime.Now;
        }

        // ... 保留你原有的 NormalizeAngle, HeadWindSpeed, CrossWindSpeed 逻辑 ...
        // 注意：HeadWindSpeed 和 CrossWindSpeed 只有 get，不需要 SetProperty，
        // 只要在 WindSpeed/Dir 的 setter 里调用 OnPropertyChanged(nameof(HeadWindSpeed)) 即可。

        private static double NormalizeAngle(double angle)
        {
            angle %= 360;
            if (angle > 180) angle -= 360;
            if (angle < -180) angle += 360;
            return angle;
        }


        public string HeadWindSpeed
        {
            get {
                double delta = NormalizeAngle(WindDir - _runwayDir);

                // 顶风分量 = V * cos(θ)
                // 正：顶风，负：顺风
                if (delta < 0)
                {

                    return $"-{Math.Round(WindSpeed * Math.Cos(delta * Math.PI / 180.0), 1)}";
                }
                return $"{Math.Round(WindSpeed * Math.Cos(delta * Math.PI / 180.0), 1)}";
            }
        }

        // 计算侧风分量（需要传入跑道方向）
        public string CrossWindSpeed
        {
            get {
                double delta = NormalizeAngle(WindDir - _runwayDir);

                // 侧风分量 = V * sin(θ)
                // 正：右侧风，负：左侧风
                double cw = WindSpeed * Math.Sin(delta * Math.PI / 180.0);
                string side = cw switch
                {
                    > 0 => "R", // Right
                    < 0 => "L", // Left
                    _ => "CALM"
                };

                return $"{side}{Math.Abs(cw).ToString()}";
            }

        }

        private void ValidateRunwayDir(short runwayDir)
        {
            if (runwayDir < 0 || runwayDir > 360)
                throw new ArgumentException("跑道方向必须在0-360度之间");
        }

    }
}
