using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.Base;


namespace WindLightSimluator.ViewModels.vm
{
    public class Light : ViewModelBase
    {
        private string _lightDegree = "3";
        private int _lightIntensity = 60;
        private string _mainPV = "8000";
        private string _mainPW = "";

        public string LightDegree
        {
            get => _lightDegree;
            set => SetProperty(ref _lightDegree, value);
        }
        public int LightIntensity
        {
            get => _lightIntensity;
            set => SetProperty(ref _lightIntensity, value);
        }
        public string MainPV
        {
            get => _mainPV;
            set => SetProperty(ref _mainPV, value);
        }
        public string MainPW
        {
            get => _mainPW;
            set => SetProperty(ref _mainPW, value);
        }

    }
}
