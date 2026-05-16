using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WindLightSimluator.Model;
using WindLightSimluator.Service;
using WindLightSimluator.utils;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.ViewModels.vm;

namespace WindLightSimluator.ViewModels
{
    public partial class AirportVM : ViewModelBase
    {

        public AirportVM(DatabaseService db)
        {
            _db = db;
            Runways = new ObservableCollection<RunwayVM>();
            // 默认添加两条跑道
            var _1 = new RunwayVM();

            // start-Part
            _1.startPart = new RunwayPartVM( true,"01L",15);
            _1.startPart.Part = RunwayPartType.Start;

            _1.startPart.Wind = new WindVM(1, 090);
            _1.startPart.Statistics = new WindStatisticsVM();
            _1.startPart.RvrVis = new RvrVisVM();
            _1.startPart.Weather = new WeatherConditionVM();


            // middle-part
            _1.middlePart = new RunwayPartVM(false, "MID1", 15);
            _1.middlePart.Part = RunwayPartType.Start;

            _1.middlePart.Wind = new WindVM(1, 090);
            _1.middlePart.Statistics = new WindStatisticsVM();
            _1.middlePart.RvrVis = new RvrVisVM();
            _1.middlePart.Weather = new WeatherConditionVM();



            // end-part
            _1.endPart = new RunwayPartVM(false, "19R", 195);
            _1.endPart.Part = RunwayPartType.Start;

            _1.endPart.Wind = new WindVM(1, 090);
            _1.endPart.Statistics = new WindStatisticsVM();
            _1.endPart.RvrVis = new RvrVisVM();
            _1.endPart.Weather = new WeatherConditionVM();

            _1.selectedPart = _1.startPart;


            Runways.Add(_1);

            var _2 = new RunwayVM();
            _2.startPart = new RunwayPartVM(true, "01R", 15);
            _2.startPart.Part = RunwayPartType.Start;

            _2.startPart.Wind = new WindVM(1, 090);
            _2.startPart.Statistics = new WindStatisticsVM();
            _2.startPart.RvrVis = new RvrVisVM();
            _2.startPart.Weather = new WeatherConditionVM();


            // middle-part
            _2.middlePart = new RunwayPartVM(false, "MID1", 15);
            _2.middlePart.Part = RunwayPartType.Start;

            _2.middlePart.Wind = new WindVM(1, 090);
            _2.middlePart.Statistics = new WindStatisticsVM();
            _2.middlePart.RvrVis = new RvrVisVM();
            _2.middlePart.Weather = new WeatherConditionVM();



            // end-part
            _2.endPart = new RunwayPartVM(false, "19L", 195);
            _2.endPart.Part = RunwayPartType.Start;

            _2.endPart.Wind = new WindVM(1, 090);
            _2.endPart.Statistics = new WindStatisticsVM();
            _2.endPart.RvrVis = new RvrVisVM();
            _2.endPart.Weather = new WeatherConditionVM();

            _2.selectedPart = _2.startPart;


            Runways.Add(_2);
            SelectedRunwayVM = Runways[0];


            Qnh = 1013;
            Light = new Light();
            Light.MainPV = "5000";
            Light.LightDegree = "3";

            // 通知 UI 快捷属性已就绪
            OnPropertyChanged(nameof(FirstRunway));
            OnPropertyChanged(nameof(SecondRunway));


            StartRandomSimulation(4);
        }
    }

}

//RunwayVM
//    3*WeatherConditionVM
//        CloudFirstLayer
//        Temperature********************
//        Duepoint
//        VVIS
//        Rain1h
//        RelativeHumidity
//        SurfaceTemperature
//        Rain24h
//        QFE
//        Status
//    3*RvrVisVM
//        RvrValue***********************                  
//        VisValue***********************
//    3*WindVM
//        WindSpeed**********************
//        WindDir************************
//    3*WindStatisticsVM
//        Avg2WindSpeed
//        Avg2WindDir
//        Max2WindSpeed
//        Min2WindSpeed
//        Avg2HeadWindSpeed
//        Avg2CrossWindSpeed