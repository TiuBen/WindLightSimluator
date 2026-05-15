using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
    public class WeatherConditionVM : ViewModelBase
    {
        private string _cloudFirstLayer = "NCD";
        public string CloudFirstLayer
        {
            get => _cloudFirstLayer;
            set => SetProperty(ref _cloudFirstLayer, value);
        }

        private double _temperature = 25.4;
        public double Temperature
        {
            get => _temperature;
            set => SetProperty(ref _temperature, value);
        }

        private double _surfaceTemperature = 22.2;
        public double SurfaceTemperature
        {
            get => _surfaceTemperature;
            set => SetProperty(ref _surfaceTemperature, value);
        }

        private double _duepoint = 1.4;
        public double Duepoint
        {
            get => _duepoint;
            set => SetProperty(ref _duepoint, value);
        }

        private int _vvis;
        public int VVIS
        {
            get => _vvis;
            set => SetProperty(ref _vvis, value);
        }

        private double _rain1h = 0.0;
        public double Rain1h
        {
            get => _rain1h;
            set => SetProperty(ref _rain1h, value);
        }

        private double _relativeHumidity = 20;
        public double RelativeHumidity
        {
            get => _relativeHumidity;
            set => SetProperty(ref _relativeHumidity, value);
        }

        private double _rain24h = 0.0;
        public double Rain24h
        {
            get => _rain24h;
            set => SetProperty(ref _rain24h, value);
        }

        private double _qfe = 1017.2;
        public double QFE
        {
            get => _qfe;
            set => SetProperty(ref _qfe, value);
        }

        private string _status = "Dry";
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }



        /// <summary>
        ///  关联的属性
        /// </summary>
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
                OnPropertyChanged(nameof(IsActive));
            }
        }
        public bool? IsActive => BelongPart?.IsActive;

        private void Parent_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(RunwayPartVM.IsActive))
            {
                OnPropertyChanged(nameof(IsActive));
            }

        }
    }
}
