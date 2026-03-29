using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
    public class RvrVisViewModel : ViewModelBase
    {
        //private readonly RunwayPartViewModel _runwayColumnViewModel;

        //public RvrVisViewModel(RunwayPartViewModel runwayColumnViewModel)
        //{
        //    _runwayColumnViewModel = runwayColumnViewModel;
        //}

        private int _rvr = 2000;
        public string RvrValue
        {
            get {
                if (_rvr >= 2000)
                {
                    return $"P2000";
                }
                else
                {
                    return _rvr.ToString();
                }

            }
            set { // 2. 转换逻辑
                int newValue;
                if (int.TryParse(value, out int intValue))
                {
                    newValue = intValue >= 0 ? intValue : 0;
                }
                else if (value?.StartsWith("P", StringComparison.OrdinalIgnoreCase) == true)
                {
                    newValue = 2000;
                }
                else
                {
                    newValue = 0;
                }

                // 3. 使用 SetProperty 触发更新
                // 注意：因为 RvrValue 是 string，而字段是 int，
                // 我们需要手动判断并调用 OnPropertyChanged
                if (_rvr != newValue)
                {
                    _rvr = newValue;
                    OnPropertyChanged(); // 自动识别属性名 "RvrValue"
                }
            }
        }

        private int _vis = 2000;
        public int VisValue
        {
            get { return _vis; }
            // 4. 标准属性直接使用 SetProperty，更简洁
            set => SetProperty(ref _vis, value);
        }

        public RvrVisViewModel(string initialRvr = "P2000", int initialVis = 2000)
        {
            // 直接走属性赋值，触发解析逻辑
            RvrValue = initialRvr;
            VisValue = initialVis;
        }

    }
}
