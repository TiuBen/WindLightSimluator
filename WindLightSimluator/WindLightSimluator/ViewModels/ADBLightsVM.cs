using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels
{
  public enum Degree
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }

   public enum DayOrNight
    {
        Day,
        Night
    }

    public class LightItemVM : ViewModelBase
    {
        private bool _isOn;
        private Degree _degree;
        private int _index;

        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        public bool IsOn
        {
            get => _isOn;
            set => SetProperty(ref _isOn, value);
        }

        public Degree Degree
        {
            get => _degree;
            set => SetProperty(ref _degree, value);
        }

        public string Name => $"Light_{Index}";

        public LightItemVM(int index)
        {
            _index = index;
            _isOn = false;
            _degree = Degree.One;
        }
    }


    public class ADBLightsVM : ViewModelBase
    {
        private DayOrNight _timeMode;
        private bool _isSpecialTypeEnabled;

        public List<LightItemVM> Lights { get; private set; } = new List<LightItemVM>();

        public DayOrNight TimeMode
        {
            get => _timeMode;
            set => SetProperty(ref _timeMode, value);
        }

        public bool IsSpecialTypeEnabled
        {
            get => _isSpecialTypeEnabled;
            set => SetProperty(ref _isSpecialTypeEnabled, value);
        }

        public ADBLightsVM()
        {
            // 初始化5个灯光
            for (int i = 0; i < 5; i++)
            {
                Lights.Add(new LightItemVM(i + 1));
            }
        }

        // 可选：添加一些便捷方法
        public void TurnAllLightsOn()
        {
            foreach (var light in Lights)
            {
                light.IsOn = true;
            }
        }

        public void TurnAllLightsOff()
        {
            foreach (var light in Lights)
            {
                light.IsOn = false;
            }
        }

        public void SetAllLightsToDegree(Degree degree)
        {
            foreach (var light in Lights)
            {
                light.Degree = degree;
            }
        }
    }

}
