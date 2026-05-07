using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WindLightSimluator.Service;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels
{
    public class MainWindowVM:ViewModelBase
    {
        private DatabaseService _db = DatabaseService.Instance;

        public AirportVM Airport { get; set; }

        public EditableWeatherElementViewModel WeatherEditor { get; set; }

        public ADBLightsVM Lights { get; set; }


        private readonly DispatcherTimer _clock_timer;

        private string _bjtTime;
        public string BjtTime
        {
            get => _bjtTime;
            set => SetProperty(ref _bjtTime, value);
        }

        private string _utcTime;
        public string UtcTime
        {
            get => _utcTime;
            set => SetProperty(ref _utcTime, value);
        }

        public MainWindowVM()
        {
            Airport = new AirportVM(_db);

            WeatherEditor = new EditableWeatherElementViewModel(_db, Airport);

            Lights = new ADBLightsVM();




            _clock_timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _clock_timer.Tick += (s, e) =>
            {
                BjtTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                UtcTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            };

            _clock_timer.Start();

        }
    }
}