using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindLightSimluator.ViewModels.vm;
using System.Collections.ObjectModel;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.Service;
using System.Windows.Threading;
using WindLightSimluator.Model;
using System.Reflection;
using WindLightSimluator.ViewModels;

namespace WindLightSimluator.ViewModels
{
    public class AirportVM : ViewModelBase
    {
        private float _qnh = 1013.2f; // 默认值
        private string _metar = "METAR ZHEC 070900Z 32002MPS CAVOK 16/05 Q1023 NOSIG=";


        public float Qnh
        {
            get => (float)Math.Round(_qnh, 1);

            set {
                if (_qnh != value)
                {
                    _qnh = value;
                    OnPropertyChanged(nameof(Qnh));
                }
            }
        }
        public string Metar
        {
            get => _metar;
            set => SetProperty(ref _metar, value);
        }

        private Light _light;
        public Light Light
        {
            get => _light;
            set => SetProperty(ref _light, value);

        }


        public ObservableCollection<RunwayVM> Runways { get; set; }
        public RunwayVM FirstRunway => Runways.Count > 0 ? Runways[0] : null;
        public RunwayVM SecondRunway => Runways.Count > 1 ? Runways[1] : null;

        private RunwayVM _selectedRunwayVM;

        public RunwayVM SelectedRunwayVM
        {
            get => _selectedRunwayVM;
            set {
                if (_selectedRunwayVM != value)
                {
                    _selectedRunwayVM = value;
                    OnPropertyChanged(nameof(SelectedRunwayVM));
                }
            }
        }



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


            _1.startPart.wind = new WindVM(1, 020, 015);
            _1.startPart.wind.IsActive = true;
            _1.startPart.statistics = new WindStatisticsVM(015, 5);
            _1.startPart.statistics.DirRangeSet = new HashSet<int> { 0, 2, 3, 4 };
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





            StartSimulation(2);
        }

        private DispatcherTimer? _timer;
        private int _counter = 0;

        public void StartSimulation(double intervalSeconds = 60)
        {
            if (_timer == null)
            {
                _timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(intervalSeconds)
                };
                _timer.Tick += Timer_Tick;
            }
            _timer.Start();
        }



        public void PauseSimulation() => _timer?.Stop();

        public void RestartSimulation()
        {
            _counter = 0;
            StartSimulation();
        }


        private void Timer_Tick(object? sender, EventArgs e)
        {
            _counter++;

            var _realTimeWindDirection = new Random().Next(0, 361); // 0到360度
            var _realTimeWindSpeed = new Random().Next(0, 21); // 0到20 m/s

            var _realTimeWindDirection1 = new Random().Next(0, 361); // 0到360度
            var _realTimeWindSpeed1 = new Random().Next(0, 21); // 0到20 m/s

            var _realTimeWindDirection2 = new Random().Next(0, 361); // 0到360度
            var _realTimeWindSpeed2 = new Random().Next(0, 21); // 0到20 m/s

            var _realTimeWindDirection3 = new Random().Next(0, 361); // 0到360度
            var _realTimeWindSpeed3 = new Random().Next(0, 21); // 0到20 m/s

            var _realTimeWindDirection4 = new Random().Next(0, 361); // 0到360度
            var _realTimeWindSpeed4 = new Random().Next(0, 21); // 0到20 m/s

            var _realTimeTemperature = new Random().Next(-20, 51); // -20到50 ℃
            var _realTimeQNH = new Random().Next(980, 1041); // 980到1040 hPa
            var _realTimeRVR = new Random().Next(0, 2501); // 0到2500 m
            var _realTimeVIS = new Random().Next(0, 15001); // 0到15000 m

            Qnh = new Random().Next(980, 1041);


            foreach (var runway in Runways)
            {

                runway.startPart.weather.Temperature = _realTimeTemperature;
                runway.middlePart.weather.Temperature = _realTimeTemperature;
                runway.endPart.weather.Temperature = _realTimeTemperature;

                runway.startPart.rvrVis.RvrValue = _realTimeRVR.ToString();
                runway.middlePart.rvrVis.RvrValue = _realTimeRVR.ToString();
                runway.endPart.rvrVis.RvrValue = _realTimeRVR.ToString();

                runway.startPart.rvrVis.VisValue = (int)_realTimeVIS;
                runway.middlePart.rvrVis.VisValue = (int)_realTimeVIS;
                runway.endPart.rvrVis.VisValue = (int)_realTimeVIS;


                runway.startPart.wind.WindSpeed = _realTimeWindSpeed;
                runway.middlePart.wind.WindSpeed = _realTimeWindSpeed;
                runway.endPart.wind.WindSpeed = _realTimeWindSpeed;

                runway.startPart.wind.WindDir = _realTimeWindDirection;
                runway.middlePart.wind.WindDir = _realTimeWindDirection;
                runway.endPart.wind.WindDir = _realTimeWindDirection;

                runway.startPart.statistics.AddSample(_realTimeWindSpeed, _realTimeWindDirection);
                runway.startPart.statistics.AddSample(_realTimeWindSpeed1, _realTimeWindDirection1);
                runway.startPart.statistics.AddSample(_realTimeWindSpeed2, _realTimeWindDirection2);
                runway.startPart.statistics.AddSample(_realTimeWindSpeed3, _realTimeWindDirection3);
                runway.startPart.statistics.AddSample(_realTimeWindSpeed4, _realTimeWindDirection4);


                runway.middlePart.statistics.AddSample(_realTimeWindSpeed, _realTimeWindDirection);
                runway.middlePart.statistics.AddSample(_realTimeWindSpeed1, _realTimeWindDirection1);
                runway.middlePart.statistics.AddSample(_realTimeWindSpeed2, _realTimeWindDirection2);
                runway.middlePart.statistics.AddSample(_realTimeWindSpeed3, _realTimeWindDirection3);
                runway.middlePart.statistics.AddSample(_realTimeWindSpeed4, _realTimeWindDirection4);

                runway.endPart.statistics.AddSample(_realTimeWindSpeed, _realTimeWindDirection);
                runway.endPart.statistics.AddSample(_realTimeWindSpeed1, _realTimeWindDirection1);
                runway.endPart.statistics.AddSample(_realTimeWindSpeed2, _realTimeWindDirection2);
                runway.endPart.statistics.AddSample(_realTimeWindSpeed3, _realTimeWindDirection3);
                runway.endPart.statistics.AddSample(_realTimeWindSpeed4, _realTimeWindDirection4);




            }




        }


        // 数据库相关的东西
        private readonly DatabaseService _db;

        public ObservableCollection<string> Tables { get; set; } = new ObservableCollection<string>();

        private string _selectedTableName;
        public string SelectedTableName
        {
            get => _selectedTableName;
            set { 
                SetProperty(ref _selectedTableName, value); 
                OnPropertyChanged(nameof(CanStart)); 
            }
        }

        public bool CanStart => !string.IsNullOrEmpty(SelectedTableName);
        public void RefreshTables()
        {


            Tables.Clear();

            foreach (var table in _db.GetTableNames())
            {
                Tables.Add(table);
            }
        }

        public void XXXX()
        {
            var data=_db.Query


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