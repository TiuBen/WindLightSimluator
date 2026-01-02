using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;

namespace WindLightSimluator.ViewModels.vm
{
   public class RvrVisViewModel : ViewModelBase
    {
        private readonly RunwayColumnViewModel _runwayColumnViewModel;

        public RvrVisViewModel(RunwayColumnViewModel runwayColumnViewModel)
        {
            _runwayColumnViewModel = runwayColumnViewModel;
        }

    }
}
