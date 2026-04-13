using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.ViewModels.vm;

namespace WindLightSimluator.ViewModels
{
    public class WindPanelVM:ViewModelBase
    {
        public WindVM  Wind { get; set; }
        public WindStatisticsVM Statistics { get; set; }

        public WindPanelVM(WindVM wind, WindStatisticsVM stats)
        {
            Wind = wind;
            Statistics = stats;
        }


        private bool? _IsActive;
        public bool? IsActive
        {
            get => _IsActive;
            set => SetProperty(ref _IsActive, value);

        }
    }
}
