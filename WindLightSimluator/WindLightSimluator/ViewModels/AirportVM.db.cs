using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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













        private DispatcherTimer _simTimer;
        private int _tick = 0;

        public void start(Dictionary<string, ObservableCollection<double>> fakeData)
        {
            // 参数验证
            if (fakeData == null)
                throw new ArgumentNullException(nameof(fakeData));

            //
            var _fakeData = fakeData;

            Random r = new();

            foreach (var runway in Runways)
            {
                runway.startPart.Offset = r.Next(0, 15);
                runway.middlePart.Offset = r.Next(5, 25);
                runway.endPart.Offset = r.Next(10, 35);
            }

            StartSimulation(_fakeData);


        }


        private void StartSimulation(Dictionary<string, ObservableCollection<double>> fakeData)
        {
            _simTimer = new DispatcherTimer();
            _simTimer.Interval = TimeSpan.FromSeconds(5);

            _simTimer.Tick += (_, __) =>
            {
                UpdateAllRunways(fakeData);

                _tick++;
            };

            _simTimer.Start();
        }



        private void UpdateAllRunways(Dictionary<string, ObservableCollection<double>> fakeData)
        {
            foreach (var runway in Runways)
            {
                UpdatePart(runway.startPart, fakeData);
                UpdatePart(runway.middlePart, fakeData);
                UpdatePart(runway.endPart, fakeData);
            }
        }

        private void UpdatePart( RunwayPartVM part, Dictionary<string, ObservableCollection<double>> fakeData)
        {
            int index = (_tick + part.Offset) % 120;

            // 风向
            part.wind.WindDir = (int)fakeData["WindDirection"][index];

            // 风速
            part.wind.WindSpeed = fakeData["WindSpeed"][index];

            part.statistics.AddSample(fakeData["WindSpeed"][index], (int)fakeData["WindDirection"][index]);

            // RVR
            part.rvrVis.Rvr =(int)fakeData["RVR"][index];

            // VIS
            part.rvrVis.VisValue = (int)fakeData["VIS"][index];

            // 温度
            part.weather.Temperature =fakeData["Temperature"][index];

            // QNH
            Qnh =(float)fakeData["QNH"][index];
        }


       
    }
}
