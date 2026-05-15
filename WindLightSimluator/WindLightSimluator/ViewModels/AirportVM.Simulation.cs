using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels
{
    public partial class AirportVM : ViewModelBase
    {
        // ============================================
        // 模拟相关字段
        // ============================================
        private DispatcherTimer _simTimer;
        private int _sensorTick = 0;
        private int _minuteIndex = 0;
        private int _elapsedSeconds = 0;

        private readonly Random _random = new();


        /// <summary>
        /// 使用数据库数据启动模拟
        /// </summary>
        public void StartDatabaseSimulation(int intervalSeconds = 5)
        {



        }
    }
}
