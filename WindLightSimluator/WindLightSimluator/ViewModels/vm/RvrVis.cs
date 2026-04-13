using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
    public class RvrVisVM : ViewModelBase
    {


        private int _rvr = 2000;
        public string RvrValue
        {
            get {
                int roundedRvr;
                if (_rvr < 400)
                {
                    // 400 以下按 25 的倍数四舍五入
                    roundedRvr = (int)Math.Round(_rvr / 25.0) * 25;
                }
                else if (_rvr <= 550)
                {
                    // 400-550 按 50 的倍数四舍五入
                    roundedRvr = (int)Math.Round(_rvr / 50.0) * 50;
                }
                else
                {
                    // 550 以上按 100 的倍数四舍五入
                    roundedRvr = (int)Math.Round(_rvr / 100.0) * 100;
                }

                if (roundedRvr >= 2000)
                {
                    return "P2000";
                }
                else
                {
                    return roundedRvr.ToString();
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
                    OnPropertyChanged(nameof(RvrValue)); // 自动识别属性名 "RvrValue"
                    //SetProperty(ref _rvr, newValue);
                }
            }
        }

        private int _vis = 2000;
        public int VisValue
        {
            get {  // 对 _vis 按 1000 的倍数进行四舍五入
                return (int)Math.Round(_vis / 1000.0) * 1000;
            }
            // 4. 标准属性直接使用 SetProperty，更简洁
            set => SetProperty(ref _vis, value);
        }

        public RvrVisVM(string initialRvr = "P2000", int initialVis = 2000)
        {
            // 直接走属性赋值，触发解析逻辑
            RvrValue = initialRvr;
            VisValue = initialVis;
        }


        private bool? _IsActive;
        public bool? IsActive
        {
            get => _IsActive;
            set => SetProperty(ref _IsActive, value);

        }

    }
}
