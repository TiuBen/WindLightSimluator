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
    public enum InUseRunwayPartEnum
    {
        Start,
        End
    }

    public class RunwayVM : ViewModelBase
    {

        public int Id { get; set; }
        // --- 跑道基础信息 ---
        private string _startPartRunwayNumber = "01";
        public string StartPartRunwayNumber
        {
            get => _startPartRunwayNumber;
            set => SetProperty(ref _startPartRunwayNumber, value);
        }

        private string _endPartRunwayNumber = "19";
        public string EndPartRunwayNumber
        {
            get => _endPartRunwayNumber;
            set => SetProperty(ref _endPartRunwayNumber, value);
        }

        private int _startPartRunwayHeading = 10;
        public int StartPartRunwayHeading
        {
            get => _startPartRunwayHeading;
            set => SetProperty(ref _startPartRunwayHeading, value);
        }

        private int _endPartRunwayHeading = 190;
        public int EndPartRunwayHeading
        {
            get => _endPartRunwayHeading;
            set => SetProperty(ref _endPartRunwayHeading, value);
        }

        private InUseRunwayPartEnum inUsePart { set; get; } = InUseRunwayPartEnum.Start;


        // 三段runway part vm 
        public RunwayPartVM startPart { get; set; }
        public RunwayPartVM middlePart { get; set; }
        public RunwayPartVM endPart { get; set; }


        public RunwayVM(RunwayPartVM start, RunwayPartVM middle, RunwayPartVM end)
        {
            startPart = start;
            middlePart = middle;
            endPart = end;
        }




















        // --- 气象条件子模块 ---
        public WeatherConditionVM StartPartWeatherConditionViewModel { get; set; } = new();
        public WeatherConditionVM MiddlePartWeatherConditionViewModel { get; set; } = new();
        public WeatherConditionVM EndPartWeatherConditionViewModel { get; set; } = new();

        // --- RvrVis子模块 ---
        public RvrVisVM StartPartRvrVisViewModel { get; set; } = new();
        public RvrVisVM MiddlePartRvrVisViewModel { get; set; } = new();
        public RvrVisVM EndPartRvrVisViewModel { get; set; } = new();






        // --- 实时风子模块 ---
        public WindVM StartPartWindViewModel { get; set; } 
        public WindVM MiddlePartWindViewModel { get; set; }
        public WindVM EndPartWindViewModel { get; set; }

        // --- 风统计子模块 ---
        // 注意：统计模块需要传入跑道航向，此处在构造函数或初始化时赋值
        public WindStatisticsVM StartPartWindStatisticViewModel { get; set; }
        public WindStatisticsVM MiddlePartWindStatisticViewModel { get; set; }
        public WindStatisticsVM EndPartPartWindStatisticViewModel { get; set; }


        public WindPanelVM StartPartWindPanelViewModel { get; set; }
        public WindPanelVM MiddlePartWindPanelViewModel { get; set; }
        public WindPanelVM EndPartWindPanelViewModel { get; set; }



        // 这里存放模拟数据源（假设每个跑道有自己的原始数据流）
        private readonly List<Wind> _dataSource;

        public RunwayVM(int id, int startHeading, string startNum, int endHeading, string endNum)
        {
            Id = id;
            StartPartRunwayHeading = startHeading;
            StartPartRunwayNumber = startNum;
            EndPartRunwayHeading = endHeading;
            EndPartRunwayNumber = endNum;
            //_dataSource = data;

            // 初始化风
            StartPartWindViewModel = new(0.4F, 110, startHeading);
            MiddlePartWindViewModel = new(1.4F, 110, startHeading);
            EndPartWindViewModel = new(1.4F, 110, startHeading);

            // 初始化Wind 统计器，传入对应的跑道磁航向
            StartPartWindStatisticViewModel = new WindStatisticsVM(startHeading,5);
            MiddlePartWindStatisticViewModel = new WindStatisticsVM(startHeading,5);
            EndPartPartWindStatisticViewModel = new WindStatisticsVM(endHeading,5);

            // 初始化 天气现象数据


            StartPartWindPanelViewModel = new WindPanelVM(StartPartWindViewModel, StartPartWindStatisticViewModel);
            MiddlePartWindPanelViewModel = new WindPanelVM(StartPartWindViewModel, StartPartWindStatisticViewModel);
            EndPartWindPanelViewModel = new WindPanelVM(StartPartWindViewModel, StartPartWindStatisticViewModel);



        }
    }
}
