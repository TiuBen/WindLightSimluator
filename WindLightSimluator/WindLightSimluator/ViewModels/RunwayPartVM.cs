using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindLightSimluator.utils;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.ViewModels.vm;

namespace WindLightSimluator.ViewModels
{
    public enum RunwayPartType
    {
        Start,  // 起始端 (例如 01L)
        Middle, // 中间段
        End     // 末端 (例如 19R)
    }

    public partial class RunwayPartVM : ViewModelBase
    {


        private string _partName;
        public string PartName
        {
            get => _partName;
            set => SetProperty(ref _partName, value);
        }

        private double _partDirection;
        public double PartDirection
        {
            get => _partDirection;
            set => SetProperty(ref _partDirection, value);
        }

        private bool? _isActive;
        public bool? IsActive
        {
            get => _isActive;
            set {
                if (SetProperty(ref _isActive, value))
                {
                    Debug.WriteLine("_isActive_isActive_isActive_isActive_isActive");

                    OnPropertyChanged(nameof(PartStatusText));
                }
            }

        }
        

        public string PartStatusText
        {
            get {
                if (_isActive == true)
                    return "LANDING/TAKE OFF";

                if (_isActive == false)
                    return "NOT IN USE";

                return string.Empty; // null
            }

        }


        // =========================
        // ✈️ RVR 灯光等级（新增）
        // =========================
        private int _rvrLightDegree;
        public int RvrLightDegree
        {
            get => _rvrLightDegree;
            set => SetProperty(ref _rvrLightDegree, value);
        }

        private WindVM _wind;
        public WindVM Wind
        {
            get => _wind;
            set {
                if (SetProperty(ref _wind, value))
                {
                    if (_wind != null)
                    {
                        _wind.BelongPart = this;
                    }
                }
            }
        }
        private WindStatisticsVM _statistics=new WindStatisticsVM();
        public WindStatisticsVM Statistics
        {
            get => _statistics;
            //set => SetProperty(ref _statistics, value);
            set {
                if (SetProperty(ref _statistics, value))
                {
                    if (_statistics != null)
                    {
                        _statistics.BelongPart = this;
                    }
                }
            }

        }

        private RvrVisVM _rvrVis=new RvrVisVM();
        public RvrVisVM RvrVis
        {
            get => _rvrVis;
            //set => SetProperty(ref _rvrVis, value);
            set {
                if (SetProperty(ref _rvrVis, value))
                {
                    if (_rvrVis != null)
                    {
                        _rvrVis.BelongPart = this;
                    }
                }
            }
        }

        private WeatherConditionVM _weather=new WeatherConditionVM();
        public WeatherConditionVM Weather
        {
            get => _weather;
            set => SetProperty(ref _weather, value);
        }

        // =========================
        // 🎯 Command：修改 RVR 等级
        // =========================
        public ICommand SetRvrLevelCommand { get; }


        private RunwayPartType _part;
        /// <summary>
        /// 标识当前是跑道的哪一部分
        /// </summary>
        public RunwayPartType Part
        {
            get => _part;
            set => SetProperty(ref _part, value);
        }


        public RunwayPartVM(bool? isActive, String partName,double partDirection)
        {
            _isActive = isActive;
            _partName = partName;
            _partDirection= partDirection;

            SetRvrLevelCommand = new RelayCommand<int>(param =>
            {

                if (int.TryParse(param.ToString(), out int level))
                {
                    RvrLightDegree = level;
                    OnPropertyChanged(nameof(RvrLightDegree));
                }
            });
        }
    }
}
