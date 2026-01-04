using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
   public class WeatherConditionViewModel : ViewModelBase
    {
        private readonly RunwayPartViewModel _runwayColumnViewModel;
        public WeatherConditionViewModel(RunwayPartViewModel runwayColumnViewModel)
        {
            _runwayColumnViewModel = runwayColumnViewModel;
        }

        public string CloudFirstLayer { get; set; } = "NCD";

        public string Temperature { get; set; } = "14.4";

        public string VVIS { get; set; } = "";

        public string DewPoint { get; set; } = "1.4";

        public string Rain1h { get; set; } = "0.0";

        public string RelativeHumidity { get; set; } = "32";

        public string Rain24h { get; set; } = "0.0";

        public string QFE { get; set; } = "1017.2";

        public string STEMP { get; set; }= "22.2";

        public string Status { get; set; } = "Dry";

    }
}
