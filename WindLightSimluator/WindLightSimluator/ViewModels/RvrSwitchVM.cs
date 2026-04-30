using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels
{
    internal class RvrSwitchVM:ViewModelBase
    {
        private string _selectedRunwayPartName;
        public string SelectedRunwayPartName
        {
            get => _selectedRunwayPartName;
            set => SetProperty(ref _selectedRunwayPartName, value);
        }

        private string _startRvrName;
        public string StartRvrName
        {
            get => _startRvrName;
            set => SetProperty(ref _startRvrName, value);
        }

        private string _endRvrName;
        public string EndRvrName
        {
            get => _endRvrName;
            set => SetProperty(ref _endRvrName, value);
        }

        private int _selectedLightDegree;
        public int SelectedLightDegree
        {
            get => _selectedLightDegree;
            set => SetProperty(ref _selectedLightDegree, value);
        }


    }
}
