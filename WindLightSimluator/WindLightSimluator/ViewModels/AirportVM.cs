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
        private float _qnh = 1013.2f; // 默认值
        private string _metar = "METAR ZHEC 070900Z 32002MPS CAVOK 16/05 Q1023 NOSIG=";
        private Light _light;
        private RunwayVM _selectedRunwayVM;
        private readonly DatabaseService _db;


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

        public Light Light
        {
            get => _light;
            set => SetProperty(ref _light, value);

        }


        public ObservableCollection<RunwayVM> Runways { get; set; }
        public RunwayVM FirstRunway => Runways.Count > 0 ? Runways[0] : null;
        public RunwayVM SecondRunway => Runways.Count > 1 ? Runways[1] : null;


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


      private  Dictionary<string, ObservableCollection<double>> fakeData=new Dictionary<string, ObservableCollection<double>>();


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