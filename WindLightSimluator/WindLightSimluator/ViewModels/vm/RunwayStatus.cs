using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;

namespace WindLightSimluator.ViewModels.vm
{
  

    public class RunwayStatusViewModel : ViewModelBase
    {
        private readonly RunwayColumnViewModel _part;

        private string _runwayNumber;
        public string RunwayNumber
        {
            get => _runwayNumber;
            set {
                _runwayNumber = value;
                OnPropertyChanged();
            }
        }

        public bool IsActive
        {
            get {
                if (_part.IsActive==true)
                {
                    return true;
                }
                return false;
            }
        
        }
       

        public string RunwayStatusText
        {
            get {
                if (_part.IsActive == true)
                    return "LANDING/TAKE OFF";

                if (_part.IsActive == false)
                    return "NOT IN USE";

                return string.Empty; // null
            }

        }

        public RunwayStatusViewModel(RunwayColumnViewModel part, String rwyNumber)
        {
            _part = part;
            _runwayNumber = rwyNumber;
        }

    }
}
