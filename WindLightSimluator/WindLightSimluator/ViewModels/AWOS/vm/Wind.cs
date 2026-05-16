using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
    public partial class WindVM : ViewModelBase
    {
        private double _windSpeed;
        public double WindSpeedValue
        {
            get => _windSpeed;
            set {
                if (_windSpeed != value)
                {
                    _windSpeed = value;

                    OnPropertyChanged(nameof(WindSpeedValue));
                    OnPropertyChanged(nameof(WindSpeed));

                    // 联动
                    OnPropertyChanged(nameof(HeadWindSpeed));
                    OnPropertyChanged(nameof(CrossWindSpeed));

                    OnPropertyChanged(nameof(AngleIndex));
                    OnPropertyChanged(nameof(AngleHeading));

                }
            }
        }
        public string WindSpeed
        {
            get {
                if (_windSpeed == 0)
                    return "CALM";
                return Math.Round(_windSpeed, 1).ToString("0.0");
            }
        }


        private double _windDir;
        public double WindDirValue
        {
            get => _windDir;
            set {
                if (_windDir != value)
                {
                    _windDir = value;

                    OnPropertyChanged(nameof(WindDirValue));
                    OnPropertyChanged(nameof(WindDir));

                    // 联动
                    OnPropertyChanged(nameof(HeadWindSpeed));
                    OnPropertyChanged(nameof(CrossWindSpeed));

                    OnPropertyChanged(nameof(AngleIndex));
                    OnPropertyChanged(nameof(AngleHeading));
                }
            }
        }
        public int WindDir
        {
            get {
                if (_windDir == 0)
                    return 360;
                return (short)(Math.Round(_windDir / 10.0) * 10);
            }
        }


        public DateTime Timestamp { get; private set; }


        private RunwayPartVM? _parent;

        public RunwayPartVM? BelongPart
        {
            get => _parent;
            set {
                // 取消旧订阅
                if (_parent != null){
                    _parent.PropertyChanged -= Parent_PropertyChanged;
                }

                _parent = value;

                // 监听新父对象
                if (_parent != null) {
                    _parent.PropertyChanged += Parent_PropertyChanged;
                }

                // 刷新顶风/侧风
                OnPropertyChanged(nameof(HeadWindSpeed));
                OnPropertyChanged(nameof(CrossWindSpeed));
            }
        }
        public double RunwayDirValue => BelongPart?.PartDirection ?? -1;




        public WindVM(double speed, double dir)
        {
            if (speed < 0)
                throw new ArgumentException("风速不能为负数");
            if (dir < 0 || dir > 360)
                throw new ArgumentException("风向必须在0-360度之间");

            _windSpeed = speed;
            _windDir = dir;
            Timestamp = DateTime.Now;
        }

        public bool? IsActive => BelongPart?.IsActive;

        public string HeadWindSpeed
        {
            get {
                if (RunwayDirValue < 0)
                    return "ERROR";

                double delta = _windDir - RunwayDirValue;
                double hw = _windSpeed * Math.Cos(delta * Math.PI / 180.0);

                double value = Math.Round(Math.Abs(hw), 1);

                if (value == 0)
                    return "CALM";


                return hw >= 0
                    ? $"{value}"   // 逆风
                    : $"-{value}";  // 顺风
            }
        }

        public string CrossWindSpeed
        {
            get {
                if (RunwayDirValue < 0)
                    return "ERROR";


                double delta = _windDir - RunwayDirValue;
                double cw = _windSpeed * Math.Sin(delta * Math.PI / 180.0);

                double value = Math.Round(Math.Abs(cw), 1);

                if (value == 0)
                    return "CALM";

                string side = cw > 0 ? "R" : "L";

                return $"{side}{value}";
            }
        }

        private void Parent_PropertyChanged( object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName ==  nameof(RunwayPartVM.PartDirection)) {
                OnPropertyChanged(nameof(HeadWindSpeed));
                OnPropertyChanged(nameof(CrossWindSpeed));
            }
            if (e.PropertyName ==nameof(RunwayPartVM.IsActive))
            {
                OnPropertyChanged(nameof(IsActive));
            }

        }


    }
}



// 注意：HeadWindSpeed 和 CrossWindSpeed 只有 get，不需要 SetProperty，
// 只要在 WindSpeed/Dir/RunwayDir 的 setter 里调用
// OnPropertyChanged(nameof(HeadWindSpeed)) 即可。

//public string HeadWindSpeed
//{
//    get {
//        if (_runwayDir <= 0)
//            return "ERROR";
//        double delta = _windDir - _runwayDir;
//        double hw = _windSpeed * Math.Cos(delta * Math.PI / 180.0);

//        double value = Math.Round(Math.Abs(hw), 1);

//        if (value == 0)
//            return "CALM";


//        return hw >= 0
//            ? $"{value}"   // 逆风
//            : $"-{value}";  // 顺风
//    }
//}

//// 计算侧风分量（需要传入跑道方向）
//public string CrossWindSpeed
//{
//    get {
//        if (_runwayDir <= 0)
//            return "ERROR";

//        double delta = _windDir - _runwayDir;
//        double cw = _windSpeed * Math.Sin(delta * Math.PI / 180.0);

//        double value = Math.Round(Math.Abs(cw), 1);

//        if (value == 0)
//            return "CALM";

//        string side = cw > 0 ? "R" : "L";

//        return $"{side}{value}";
//    }

//}