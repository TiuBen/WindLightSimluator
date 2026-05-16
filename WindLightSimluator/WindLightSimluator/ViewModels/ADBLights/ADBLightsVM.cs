using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using WindLightSimluator.utils;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels
{
    public enum Degree
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }

    public enum DayOrNight
    {
        Day=0,
        Night=1
    }

    // 定义灯光状态枚举
    public enum LightStatus
    {
        Closed = 0,   // 关闭
        Landing = 1,  // 降落
        TakeOff = 2   // 起飞
    }


    public class LightItemVM : ViewModelBase
    {
        private int _index;
        private string _lightName;
        private LightStatus _lightStatus;
        private Degree _degree;
        private DayOrNight _isNight;

        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        public LightStatus LightStatus
        {
            get => _lightStatus;
            set {
                if (SetProperty(ref _lightStatus, value))
                {
                    OnPropertyChanged(nameof(LightStatusText));
                    OnPropertyChanged(nameof(IsLightOn));
                }
            }
        }

        public DayOrNight DayOrNight
        {
            get => _isNight;
            set {
                if (SetProperty(ref _isNight, value))
                {
                    OnPropertyChanged(nameof(DayOrNightText));
                }
            }
        }

        public Degree Degree
        {
            get => _degree;
            set {
                if (SetProperty(ref _degree, value))
                {
                    OnPropertyChanged(nameof(DegreeText));
                }
            }
        }

        public string Name => _lightName;

        public LightItemVM(string name, LightStatus isLightOn)
        {
            _lightName = name;
            _lightStatus = isLightOn;
            _degree = Degree.One;
            _isNight = DayOrNight.Day;
        }

        // ✅ 响应 Degree
        public string DegreeText => $"{(int)Degree}档";

        // ✅ 修正逻辑 + 响应 DayOrNight
        public string DayOrNightText => _isNight == DayOrNight.Day ? "白天" : "夜晚";

        // ✅ 响应 LightStatus
        public string LightStatusText
        {
            get {
                if (_lightName == "机坪")
                {
                    return "机坪";
                }

                return _lightStatus switch
                {
                    LightStatus.Closed => $"{_lightName} 关闭",
                    LightStatus.Landing => $"{_lightName} 降落",
                    _ => $"{_lightName} 起飞"
                };
            }
        }

        // ✅ 响应 LightStatus
        public bool IsLightOn => _lightStatus != LightStatus.Closed;

        public LightItemVM Clone()
        {
            return new LightItemVM(_lightName, _lightStatus)
            {
                Index = Index,
                Degree = Degree,
                DayOrNight = DayOrNight
            };
        }
        public void ApplyFrom(LightItemVM source)
        {
            if (source == null) return;

            LightStatus = source.LightStatus;
            Degree = source.Degree;
            DayOrNight = source.DayOrNight;
        }


    }


    public class ADBLightsVM : ViewModelBase
    {
        private DayOrNight _timeMode;
        private bool _isCat2Enabled;
        private int _selectedLightIndex;
        private LightItemVM _selectedLightVM;  // 新增：当前选中的灯光


        public DayOrNight TimeMode
        {
            get => _timeMode;
            set => SetProperty(ref _timeMode, value);
        }



        public List<LightItemVM> Lights { get; private set; } = new List<LightItemVM>();
        public bool IsCat2Enabled
        {
            get => _isCat2Enabled;
            set => SetProperty(ref _isCat2Enabled, value);
        }

      

        public int SelectedLightIndex
        {
            get => _selectedLightIndex;
            set {
                if (SetProperty(ref _selectedLightIndex, value))
                {
                    if (value >= 0 && value < Lights.Count)
                    {
                        SelectedLightVM = Lights[value].Clone();
                    }
                }
            }
        }

        // 新增：选中的灯光VM属性
        public LightItemVM SelectedLightVM
        {
            get => _selectedLightVM;
            set => SetProperty(ref _selectedLightVM, value);
        }


        public ADBLightsVM()
        {
            // 初始化5个灯光
            Lights.Add(new LightItemVM("01L", LightStatus.Closed));
            Lights.Add(new LightItemVM("19R", LightStatus.Landing));
            Lights.Add(new LightItemVM("01R", LightStatus.Landing));
            Lights.Add(new LightItemVM("19L", LightStatus.Landing));
            Lights.Add(new LightItemVM("机坪", LightStatus.Landing));
            // 可选：默认选中第一个灯光
            SelectedLightIndex = 0;
            SelectedLightVM = Lights[SelectedLightIndex].Clone();

         
        }



    }

}
