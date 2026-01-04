using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.Model;

namespace WindLightSimluator.ViewModels.vm
{
  


    public class HeadCrossWindViewModel:ViewModelBase
    {



        //private readonly RunwayViewModel _runway;
        public HeadCrossWindViewModel(RunwayViewModel part) {
            //_runway = part;
        }
        public HeadCrossWindViewModel( )
        {
            //_runway = part;
        }

        //private UI_WQT? _wqt;
        //public UI_WQT? WQT
        //{
        //    get => _wqt;
        //    set
        //    {
        //        _wqt = value;
        //        OnPropertyChanged();
        //        OnPropertyChanged(nameof(CurrentHeadWind));
        //        OnPropertyChanged(nameof(Avg2HeadWind));
        //    }
        //}

        // ===== UI 直接绑定的属性 =====


        public float Avg2HeadWindSpeed { get; set; } = 01f;
        public string Avg2CrossWindSpeed { get; set; } = "R4.5";
        public string Min2WindDir { get; set; } ="90";
        public float Min2WindSpeed { get; set; } = 0.2f;
        public string Max2WindDir { get; set; } = "90";
        public float Max2WindSpeed { get; set; } = 0.2f;
        public string Avg2WindDir { get; set; } = "90";
        public float Avg2WindSpeed { get; set; } = 0.2f;
    }
}
