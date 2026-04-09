using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
  

    public class RunwayStatusVM : ViewModelBase
    {

        private string _runwayNumber;
        public string RunwayNumber
        {
            get => _runwayNumber;
            set => SetProperty(ref _runwayNumber, value);
        }

        private bool _IsActive;
        public bool IsActive
        {
            get => _IsActive;
            set => SetProperty(ref _IsActive, value);

        }


        public string RunwayStatusText
        {
            get {
                if (IsActive == true)
                    return "LANDING/TAKE OFF";

                if (IsActive == false)
                    return "NOT IN USE";

                return string.Empty; // null
            }

        }

        public RunwayStatusVM(bool isActive, String rwyNumber)
        {
            _IsActive = isActive;
            _runwayNumber = rwyNumber;
        }

    }
}
