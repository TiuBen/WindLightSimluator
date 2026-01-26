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

        private int _rvr = 2000;
        public string RvrValue
        {
            get {
                if (_rvr >= 2000)
                {
                    return $"P2000";
                }
                else
                {
                    return _rvr.ToString();
                }

            }
            set { _rvr=value; }
        }


        public int VisValue { get; set; } = 2000;

    }
}
