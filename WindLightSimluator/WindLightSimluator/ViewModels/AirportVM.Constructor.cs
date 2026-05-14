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
            _1.startPart = new();
            _1.startPart.Part = RunwayPartType.Start;
            _1.startPart.IsActive = true;
            _1.startPart.PartName = "01L";


            _1.startPart.wind = new WindVM(1, 090, 015);
            _1.startPart.wind.IsActive = true;
            _1.startPart.statistics = new WindStatisticsVM(015, 5);
            _1.startPart.statistics.DirRangeSet = new HashSet<int> { 0,1, 2, 3, 4 };
            _1.startPart.statistics.IsActive = true;
            _1.startPart.rvrVis = new RvrVisVM();
            _1.startPart.rvrVis.IsActive = true;
            _1.startPart.weather = new WeatherConditionVM();
            _1.startPart.weather.IsActive = true;


            // middle-part
            _1.middlePart = new();
            _1.middlePart.Part = RunwayPartType.Middle;
            _1.middlePart.IsActive = null;
            _1.middlePart.PartName = "MID1";

            _1.middlePart.wind = new WindVM(1, 020, 015);
            _1.middlePart.wind.IsActive = false;
            _1.middlePart.statistics = new WindStatisticsVM(015, 5);
            _1.middlePart.statistics.DirRangeSet = new HashSet<int> { 0, 2, 3, 4 };

            _1.middlePart.statistics.IsActive = false;
            _1.middlePart.rvrVis = new RvrVisVM();
            _1.middlePart.rvrVis.IsActive = false;
            _1.middlePart.weather = new WeatherConditionVM();
            _1.middlePart.weather.IsActive = false;


            // end-part
            _1.endPart = new();
            _1.endPart.Part = RunwayPartType.End;
            _1.endPart.IsActive = false;
            _1.endPart.PartName = "19R";

            _1.endPart.wind = new WindVM(1, 020, 015);
            _1.endPart.wind.IsActive = false;
            _1.endPart.statistics = new WindStatisticsVM(015, 5);
            _1.endPart.statistics.DirRangeSet = new HashSet<int> { 0, 2, 3, 4 };

            _1.endPart.statistics.IsActive = false;
            _1.endPart.rvrVis = new RvrVisVM();
            _1.endPart.rvrVis.IsActive = false;
            _1.endPart.weather = new WeatherConditionVM();
            _1.endPart.weather.IsActive = false;


            _1.selectedPart = _1.startPart;


            Runways.Add(_1);

            var _2 = new RunwayVM();
            _2.startPart = new();
            _2.startPart.Part = RunwayPartType.Start;
            _2.startPart.IsActive = true;
            _2.startPart.PartName = "01R";


            _2.startPart.wind = new WindVM(1, 020, 015);
            _2.startPart.wind.IsActive = true;
            _2.startPart.statistics = new WindStatisticsVM(015, 5);
            _2.startPart.statistics.DirRangeSet = new HashSet<int> { 0, 2, 3, 4 };
            _2.startPart.statistics.IsActive = true;
            _2.startPart.rvrVis = new RvrVisVM();
            _2.startPart.rvrVis.IsActive = true;
            _2.startPart.weather = new WeatherConditionVM();
            _2.startPart.weather.IsActive = true;


            // middle-part
            _2.middlePart = new();
            _2.middlePart.Part = RunwayPartType.Middle;
            _2.middlePart.IsActive = null;
            _2.middlePart.PartName = "MID1";

            _2.middlePart.wind = new WindVM(1, 020, 015);
            _2.middlePart.wind.IsActive = false;
            _2.middlePart.statistics = new WindStatisticsVM(015, 5);
            _2.middlePart.statistics.DirRangeSet = new HashSet<int> { 0, 2, 3, 4 };

            _2.middlePart.statistics.IsActive = false;
            _2.middlePart.rvrVis = new RvrVisVM();
            _2.middlePart.rvrVis.IsActive = false;
            _2.middlePart.weather = new WeatherConditionVM();
            _2.middlePart.weather.IsActive = false;


            // end-part
            _2.endPart = new();
            _2.endPart.Part = RunwayPartType.End;
            _2.endPart.IsActive = false;
            _2.endPart.PartName = "19L";

            _2.endPart.wind = new WindVM(1, 020, 015);
            _2.endPart.wind.IsActive = false;
            _2.endPart.statistics = new WindStatisticsVM(015, 5);
            _2.endPart.statistics.DirRangeSet = new HashSet<int> { 0, 2, 3, 4 };

            _2.endPart.statistics.IsActive = false;
            _2.endPart.rvrVis = new RvrVisVM();
            _2.endPart.rvrVis.IsActive = false;
            _2.endPart.weather = new WeatherConditionVM();
            _2.endPart.weather.IsActive = false;



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


            StartSimulationCommand = new RelayCommand2(start);

            StopSimulationCommand = new RelayCommand2(stop);

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