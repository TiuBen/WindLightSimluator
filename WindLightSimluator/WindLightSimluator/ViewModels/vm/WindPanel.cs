using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;

namespace WindLightSimluator.ViewModels.vm
{
    public class WindPanelViewModel : ViewModelBase
    {
        private readonly RunwayColumnViewModel _part;

     

        public WindPanelViewModel(RunwayColumnViewModel part)
        {
            _part = part;
        }
    }
}
