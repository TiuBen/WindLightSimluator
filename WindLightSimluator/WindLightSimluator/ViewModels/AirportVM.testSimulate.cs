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

        public void StopRandomSimulation()
        {
            _timer?.Stop();
            _timer = null;
        }

    }
}
