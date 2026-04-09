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
        private string _light = "3";
        private int _lightIntensity = 60;
        private string _mainPV = "8000";
        private string _mainPW = "///";

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
        public string Light
        {
            get => _light;
            set => SetProperty(ref _light, value);
        }
        public int LightIntensity
        {
            get => _lightIntensity;
            set => SetProperty(ref _lightIntensity, value);
        }
        public string MainPV
        {
            get => _mainPV;
            set => SetProperty(ref _mainPV, value);
        }
        public string MainPW
        {
            get => _mainPW;
            set => SetProperty(ref _mainPW, value);
        }


        public ObservableCollection<RunwayVM> Runways { get; set; }
        public RunwayVM FirstRunway => Runways.Count > 0 ? Runways[0] : null;
        public RunwayVM SecondRunway => Runways.Count > 1 ? Runways[1] : null;


        private Dictionary<string, List<double>> RawData { get; } = new()
        {
            ["WindDirection"] = Enumerable.Repeat(180.0, 120).ToList(),
            ["WindSpeed"] = Enumerable.Repeat(2.0, 120).ToList(),
            ["Temperature"] = Enumerable.Repeat(15.0, 120).ToList(),
            ["QNH"] = Enumerable.Repeat(1013.0, 120).ToList(),
            ["RVR"] = Enumerable.Repeat(2000.0, 120).ToList(),
            ["VIS"] = Enumerable.Repeat(5000.0, 120).ToList()
        };
        public AirportVM()
        {
            Runways = new ObservableCollection<RunwayVM>();
            // 默认添加两条跑道
            Runways.Add(new RunwayVM(1, 15, "01L", 195, "19R"));
            Runways.Add(new RunwayVM(2, 15, "01R", 195, "19L"));

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
            //var _realTimeWindDirection = RawData["WindDirection"][_counter];
            //var _realTimeWindSpeed = RawData["WindSpeed"][_counter];
            //var _realTimeTemperature = RawData["Temperature"][_counter];
            //var _realTimeQNH = RawData["QNH"][_counter];
            //var _realTimeRVR = RawData["RVR"][_counter];
            //var _realTimeVIS = RawData["VIS"][_counter];

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
            
            Qnh= new Random().Next(980, 1041);


            foreach (var runway in Runways)
            {
                
                runway.StartPartWeatherConditionViewModel.Temperature = _realTimeTemperature;
                runway.MiddlePartWeatherConditionViewModel.Temperature = _realTimeTemperature;
                runway.EndPartWeatherConditionViewModel.Temperature = _realTimeTemperature;

                runway.StartPartRvrVisViewModel.RvrValue =     _realTimeRVR.ToString();
                runway.MiddlePartRvrVisViewModel.RvrValue = _realTimeRVR.ToString();
                runway.EndPartRvrVisViewModel.RvrValue = _realTimeRVR.ToString();

                runway.StartPartRvrVisViewModel.VisValue = (int)_realTimeVIS;
                runway.MiddlePartRvrVisViewModel.VisValue = (int)_realTimeVIS;
                runway.EndPartRvrVisViewModel.VisValue = (int)_realTimeVIS;


                runway.StartPartWindViewModel.WindSpeed =_realTimeWindSpeed;
                runway.MiddlePartWindViewModel.WindSpeed =_realTimeWindSpeed;
                runway.EndPartWindViewModel.WindSpeed =_realTimeWindSpeed;

                runway.StartPartWindViewModel.WindDir = _realTimeWindDirection;
                runway.MiddlePartWindViewModel.WindDir = _realTimeWindDirection;
                runway.EndPartWindViewModel.WindDir = _realTimeWindDirection;

                runway.StartPartWindStatisticViewModel.AddSample(_realTimeWindSpeed, _realTimeWindDirection);
                runway.StartPartWindStatisticViewModel.AddSample(_realTimeWindSpeed1, _realTimeWindDirection1);
                runway.StartPartWindStatisticViewModel.AddSample(_realTimeWindSpeed2, _realTimeWindDirection2);
                runway.StartPartWindStatisticViewModel.AddSample(_realTimeWindSpeed3, _realTimeWindDirection3);
                runway.StartPartWindStatisticViewModel.AddSample(_realTimeWindSpeed4, _realTimeWindDirection4);


                runway.MiddlePartWindStatisticViewModel.AddSample(_realTimeWindSpeed, _realTimeWindDirection);
                runway.MiddlePartWindStatisticViewModel.AddSample(_realTimeWindSpeed1, _realTimeWindDirection1);
                runway.MiddlePartWindStatisticViewModel.AddSample(_realTimeWindSpeed2, _realTimeWindDirection2);
                runway.MiddlePartWindStatisticViewModel.AddSample(_realTimeWindSpeed3, _realTimeWindDirection3);
                runway.MiddlePartWindStatisticViewModel.AddSample(_realTimeWindSpeed4, _realTimeWindDirection4);

                runway.EndPartPartWindStatisticViewModel.AddSample(_realTimeWindSpeed, _realTimeWindDirection);
                runway.EndPartPartWindStatisticViewModel.AddSample(_realTimeWindSpeed1, _realTimeWindDirection1);
                runway.EndPartPartWindStatisticViewModel.AddSample(_realTimeWindSpeed2, _realTimeWindDirection2);
                runway.EndPartPartWindStatisticViewModel.AddSample(_realTimeWindSpeed3, _realTimeWindDirection3);
                runway.EndPartPartWindStatisticViewModel.AddSample(_realTimeWindSpeed4, _realTimeWindDirection4);




            }
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