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
            startPart.status = new RunwayStatusVM(true, "01L");
            //startPart.status = new RunwayStatusVM();
            startPart.status.RunwayNumber ="01L" ;
            startPart.wind = new WindVM(1, 020, 015);
            startPart.statistics = new WindStatisticsVM(015, 5);
            startPart.rvrVis = new RvrVisVM();
            startPart.weather = new WeatherConditionVM();

            middlePart = new();
            middlePart.status = new RunwayStatusVM(null, "MID1");
            //middlePart.status = new RunwayStatusVM();
            middlePart.wind = new WindVM(1, 020, 015);
            middlePart.statistics = new WindStatisticsVM(015, 5);
            middlePart.rvrVis = new RvrVisVM();

            middlePart.weather = new WeatherConditionVM();


            endPart = new();
            endPart.status = new RunwayStatusVM(false, "19R");
            //endPart.status = new RunwayStatusVM();
            endPart.wind = new WindVM(1, 020, 015);
            endPart.statistics = new WindStatisticsVM(015, 5);
            endPart.rvrVis = new RvrVisVM();

            endPart.weather = new WeatherConditionVM();

        }

        // 这里存放模拟数据源（假设每个跑道有自己的原始数据流）
        private readonly List<Wind> _dataSource;

      
    }
}
