using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
   public class RvrVisViewModel : ViewModelBase
    {
        private readonly RunwayPartViewModel _runwayColumnViewModel;

        public RvrVisViewModel(RunwayPartViewModel runwayColumnViewModel)
        {
            _runwayColumnViewModel = runwayColumnViewModel;
        }

        public int RvrValue { get; set; } = 2000;
        public int VisValue { get; set; } = 2000;

    }
}
