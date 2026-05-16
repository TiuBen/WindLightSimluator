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
    public class RvrVisVM : ViewModelBase
    {


        private int _rvr = 2000;
        public int Rvr
        {
            get => _rvr;
            set {
                if (SetProperty(ref _rvr, value))
                {
                    OnPropertyChanged(nameof(RvrValue));
                }
            }
        }
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
          
        }

        private int _vis = 2000;
        public int Vis
        {
            get => _vis;
            set {
                if (SetProperty(ref _vis, value))
                {
                    OnPropertyChanged(nameof(VisValue));
                }
            }
        }
        public int VisValue
        {
            get {
                // 按 1000 四舍五入
                int result = (int)Math.Round(_vis / 1000.0) * 1000;

                // 最大 10000
                return Math.Min(result, 10000);
            }
        }

        public RvrVisVM(double initialRvr = 2000, double initialVis = 2000)
        {
            // 直接走属性赋值，触发解析逻辑
            _rvr =(int)initialRvr;
            _vis = (int)initialVis;
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
