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

    public class RunwayPartVM : ViewModelBase
    {


        private string _partName;
        public string PartName
        {
            get => _partName;
            set => SetProperty(ref _partName, value);
        }

        private bool? _isActive;
        public bool? IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);

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


        public WindVM wind { get; set; }
        public WindStatisticsVM statistics { get; set; }
        public RvrVisVM rvrVis { get; set; }
        public WeatherConditionVM weather { get; set; }

       

        private RunwayPartType _part;
        /// <summary>
        /// 标识当前是跑道的哪一部分
        /// </summary>
        public RunwayPartType Part
        {
            get => _part;
            set => SetProperty(ref _part, value);
        }


        public RunwayPartVM(bool? isActive, String partNaame)
        {
            _isActive = isActive;
            _partName = partNaame;
        }

        public RunwayPartVM()
        {
            _isActive = false;
            _partName = string.Empty;
        }


    }
}
