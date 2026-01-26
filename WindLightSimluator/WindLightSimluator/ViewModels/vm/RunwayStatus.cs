using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
  

    public class RunwayStatusViewModel : ViewModelBase
    {
        private readonly RunwayPartViewModel _part;

        private string _runwayNumber;
        public string RunwayNumber
        {
            get => _runwayNumber;
            set {
                _runwayNumber = value;
                OnPropertyChanged();
            }
        }

        public bool? IsActive
        {
            get {
                return _part.IsActive;
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

        public RunwayStatusViewModel(RunwayPartViewModel part, String rwyNumber)
        {
            _part = part;
            _runwayNumber = rwyNumber;
        }

    }
}
