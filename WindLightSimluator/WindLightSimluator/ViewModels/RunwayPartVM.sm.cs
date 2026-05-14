using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels
{
    public partial class RunwayPartVM : ViewModelBase
    {
        // 新增
        public double WindDirNoise { get; set; }

        public double WindSpeedNoise { get; set; }

        // 更新延迟
        public int SensorDelay { get; set; }

        // 更新节拍
        public int UpdatePhase { get; set; }
    }
}
