using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;

namespace WindLightSimluator.ViewModels.vm
{
   public class WeatherConditionViewModel : ViewModelBase
    {
        private readonly RunwayColumnViewModel _runwayColumnViewModel;
        public WeatherConditionViewModel(RunwayColumnViewModel runwayColumnViewModel)
        {
            _runwayColumnViewModel = runwayColumnViewModel;
        }
    }
}
