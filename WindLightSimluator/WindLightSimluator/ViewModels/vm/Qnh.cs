using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
    public class QnhViewModel : ViewModelBase
    {
        private float _qnh;

        public float Qnh
        {
            get => (float)Math.Round(_qnh, 1);

            set {
                if (_qnh != value)
                {
                    _qnh = value;
                    OnPropertyChanged(nameof(Qnh));
                }
            }
        }


    }
}
