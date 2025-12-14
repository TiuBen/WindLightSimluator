using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WindLightSimluator.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool _activeGridIsActive = true;
        public bool ActiveGridIsActive
        {
            get => _activeGridIsActive;
            set { _activeGridIsActive = value; OnPropertyChanged(); }
        }

        private bool _inactiveGridIsActive = false;
        public bool InactiveGridIsActive
        {
            get => _inactiveGridIsActive;
            set { _inactiveGridIsActive = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
