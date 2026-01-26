using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
    public class MetarViewModel : ViewModelBase
    {
        public string  Metar { get; set; }  
        public bool isNew { get; set; }


    }
}
