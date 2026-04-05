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

    public class RunwayViewModel : ViewModelBase
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

        // --- 气象条件子模块 ---
        public WeatherConditionViewModel StartPartWeatherConditionViewModel { get; set; } = new();
        public WeatherConditionViewModel MiddlePartWeatherConditionViewModel { get; set; } = new();
        public WeatherConditionViewModel EndPartWeatherConditionViewModel { get; set; } = new();

        // --- RvrVis子模块 ---
        public RvrVisViewModel StartPartRvrVisViewModel { get; set; } = new();
        public RvrVisViewModel MiddlePartRvrVisViewModel { get; set; } = new();
        public RvrVisViewModel EndPartRvrVisViewModel { get; set; } = new();

        // --- 实时风子模块 ---
        public WindViewModel  StartPartWindViewModel{ get; set; }
        public WindViewModel MiddlePartWindViewModel { get; set; }
        public WindViewModel EndPartWindViewModel { get; set; }

        // --- 风统计子模块 ---
        // 注意：统计模块需要传入跑道航向，此处在构造函数或初始化时赋值
        public WindStatisticsViewModel StartPartWindStatistic { get; set; }
        public WindStatisticsViewModel MiddlePartWindStatistic { get; set; }
        public WindStatisticsViewModel EndPartPartWindStatistic { get; set; }

        // 这里存放模拟数据源（假设每个跑道有自己的原始数据流）
        private readonly List<Wind> _dataSource;

        public RunwayViewModel(int id, int startHeading, string startNum, int endHeading, string endNum)
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
            StartPartWindStatistic = new WindStatisticsViewModel((short)startHeading);
            MiddlePartWindStatistic = new WindStatisticsViewModel((short)startHeading);
            EndPartPartWindStatistic = new WindStatisticsViewModel((short)endHeading);

            // 初始化 天气现象数据
          

        }


        public void Update(DateTime currentTime)
        {
            // 模拟从数据源中找出当前时间点的数据
            var data = _dataSource.FirstOrDefault(x => x.time <= currentTime);
            if (data != null)
            {
                // 将新数据喂给统计器，统计器会自动触发 OnPropertyChanged
                StartPartWindStatistic.AddWindSample(data.WindSpeed, (short)data.WindDir);
                MiddlePartWindStatistic.AddWindSample(data.WindSpeed, (short)data.WindDir);
                EndPartPartWindStatistic.AddWindSample(data.WindSpeed, (short)data.WindDir);
            }
        }
    }



}
