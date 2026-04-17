using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WindLightSimluator.Views.Components;
using WindLightSimluator.ViewModels.vm;
using System.Data;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.Model;

namespace WindLightSimluator.ViewModels
{
    public class RunwayVM : ViewModelBase
    {

        public int Id { get; set; }
    
        // 三段runway part vm 
        public RunwayPartVM startPart { get; set; } = new();
        public RunwayPartVM middlePart { get; set; } = new();
        public RunwayPartVM endPart { get; set; } = new();


        public RunwayVM()
        {
            startPart = new();
            startPart.Part = RunwayPartType.Start;
            startPart.status = new RunwayStatusVM(true, "01L");
            //startPart.status = new RunwayStatusVM();
            startPart.status.RunwayNumber ="01L" ;
            startPart.wind = new WindVM(1, 020, 015);
            startPart.wind.IsActive = true;
            startPart.statistics = new WindStatisticsVM(015, 5);
            startPart.statistics.DirRangeSet = new HashSet<int> { 0, 2, 3, 4 };
            startPart.statistics.IsActive = true;
            startPart.rvrVis = new RvrVisVM();
            startPart.rvrVis.IsActive = true;
            startPart.weather = new WeatherConditionVM();
            startPart.weather.IsActive = true;

            middlePart = new();
            middlePart.Part = RunwayPartType.Middle;
            middlePart.status = new RunwayStatusVM(null, "MID1");
            //middlePart.status = new RunwayStatusVM();
            middlePart.wind = new WindVM(1, 020, 015);
            middlePart.wind.IsActive = false;
            middlePart.statistics = new WindStatisticsVM(015, 5);
            middlePart.statistics.DirRangeSet = new HashSet<int> { 0, 2, 3, 4 };

            middlePart.statistics.IsActive = false;
            middlePart.rvrVis = new RvrVisVM();
            middlePart.rvrVis.IsActive= false;
            middlePart.weather = new WeatherConditionVM();
            middlePart.weather.IsActive = false;


            endPart = new();
            endPart.Part = RunwayPartType.End;
            endPart.status = new RunwayStatusVM(false, "19R");
            endPart.wind = new WindVM(1, 020, 015);
            endPart.wind.IsActive = false;
            endPart.statistics = new WindStatisticsVM(015, 5);
            endPart.statistics.DirRangeSet = new HashSet<int> { 0, 2, 3, 4 };

            endPart.statistics.IsActive = false;
            endPart.rvrVis = new RvrVisVM();
            endPart.rvrVis.IsActive = false;
            endPart.weather = new WeatherConditionVM();
            endPart.weather.IsActive = false;

        }

        // 这里存放模拟数据源（假设每个跑道有自己的原始数据流）
        private readonly List<Wind> _dataSource;

      
    }
}
