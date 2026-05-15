using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.ViewModels.vm;

namespace WindLightSimluator.ViewModels
{
    public partial class AirportVM : ViewModelBase
    {

        private DispatcherTimer? _timer;

        public void StartRandomSimulation(double intervalSeconds = 4)
        {
            if (_timer == null)
            {
                _timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(intervalSeconds)
                };
                _timer.Tick += Random_Timer_Tick;
            }
            _timer.Start();
        }

        private void Random_Timer_Tick(object? sender, EventArgs e)
        {
            var _realTimeWindDirection = new Random().Next(1, 360); // 0到20 m/s
            var _realTimeWindSpeed = new Random().Next(0, 21); // 0到20 m/s

            var _realTimeWindDirection1 = new Random().Next(1, 36); // 0到360度
            var _realTimeWindSpeed1 = new Random().Next(0, 21); // 0到20 m/s

            var _realTimeWindDirection2 = new Random().Next(1, 360); // 0到360度
            var _realTimeWindSpeed2 = new Random().Next(0, 21); // 0到20 m/s

           

            var _realTimeTemperature = new Random().Next(-20, 51); // -20到50 ℃
            var _realTimeQNH = new Random().Next(980, 1041); // 980到1040 hPa
            var _realTimeRVR = new Random().Next(0, 2501); // 0到2500 m
            var _realTimeVIS = new Random().Next(0, 15001); // 0到15000 m

            Qnh = new Random().Next(980, 1041);


            foreach (var runway in Runways)
            {

                runway.startPart.Weather.Temperature = _realTimeTemperature;
                runway.middlePart.Weather.Temperature = _realTimeTemperature;
                runway.endPart.Weather.Temperature = _realTimeTemperature;

                runway.startPart.RvrVis.RvrValue = _realTimeRVR.ToString();
                runway.middlePart.RvrVis.RvrValue = _realTimeRVR.ToString();
                runway.endPart.RvrVis.RvrValue = _realTimeRVR.ToString();

                runway.startPart.RvrVis.VisValue = (int)_realTimeVIS;
                runway.middlePart.RvrVis.VisValue = (int)_realTimeVIS;
                runway.endPart.RvrVis.VisValue = (int)_realTimeVIS;


                var Wind = new WindVM(_realTimeWindSpeed, _realTimeWindDirection);
                var Wind1 = new WindVM(_realTimeWindSpeed1, _realTimeWindDirection1);
                var Wind2 = new WindVM(_realTimeWindSpeed2, _realTimeWindDirection2);
                runway.startPart.Wind = Wind;
                runway.middlePart.Wind = Wind1;
                runway.endPart.Wind = Wind2;



                runway.startPart.Statistics.AddWindVM(Wind);
                runway.middlePart.Statistics.AddWindVM(Wind1);
                runway.endPart.Statistics.AddWindVM(Wind2);
        



            }




        }

        public void StopRandomSimulation()
        {
            _timer?.Stop();
            _timer = null;
        }

    }
}
