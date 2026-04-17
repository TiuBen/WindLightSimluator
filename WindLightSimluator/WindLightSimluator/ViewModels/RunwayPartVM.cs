using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public class RunwayPartVM:ViewModelBase
    {

        public RunwayStatusVM status { get; set; } = new();
        public WindVM wind { get; set; } 
        public WindStatisticsVM statistics { get; set; }
        public RvrVisVM rvrVis { get; set; }
        public WeatherConditionVM weather { get; set; }

        private bool? _IsActive;
        public bool? IsActive
        {
            get => _IsActive;
            set => SetProperty(ref _IsActive, value);

        }

        private RunwayPartType _part;
        /// <summary>
        /// 标识当前是跑道的哪一部分
        /// </summary>
        public RunwayPartType Part
        {
            get => _part;
            set => SetProperty(ref _part, value);
        }

    }
}
