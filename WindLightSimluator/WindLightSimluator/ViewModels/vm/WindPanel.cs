using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
    public class WindPanelViewModel : ViewModelBase
    {
        private readonly RunwayPartViewModel _part;


        private HashSet<int> _rangeArcIndex = new();
        public HashSet<int> RangeArcIndex
        {
            get => _rangeArcIndex;
            set
            {
                _rangeArcIndex = value;
                OnPropertyChanged();
            }
        }

        private int _directedArcIndex;
        public int DirectedArcIndex
        {
            get => _directedArcIndex;
            set
            {
                if (_directedArcIndex != value)
                {
                    _directedArcIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged();
                }
            }
        }


        public WindPanelViewModel(RunwayPartViewModel part)
        {
            _part = part;
        }
    }
}
