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



    public class HeadCrossWindViewModel : ViewModelBase
    {



        private readonly RunwayViewModel _runway;
        public HeadCrossWindViewModel(RunwayViewModel part)
        {
            _runway = part;
        }
        public void UpdateFrom(WQTStatisticSnapshot snapshot2minWQT)
        {
            if (snapshot2minWQT == null) return; ;
            Avg2HeadWindSpeed = snapshot2minWQT.Avg2Wind.HeadWindSpeed.ToString();  // 假设有这样的属性
            Avg2CrossWindSpeed = snapshot2minWQT.Avg2Wind.CrossWindSpeed.ToString();  // 示例
            Min2WindDir = snapshot2minWQT.Min2Wind.WindDir.ToString();
            Min2WindSpeed = snapshot2minWQT.Min2Wind.WindSpeed.ToString();
            Max2WindDir = snapshot2minWQT.Max2Wind.WindDir.ToString();
            Max2WindSpeed = snapshot2minWQT.Max2Wind.WindSpeed.ToString();
            Avg2WindDir = snapshot2minWQT.Avg2Wind.WindDir.ToString();
            Avg2WindSpeed = snapshot2minWQT.Avg2Wind.WindSpeed.ToString();

        }



        // ===== UI 直接绑定的属性 =====


        public string Avg2HeadWindSpeed { get; set; } = "12";
        public string Avg2CrossWindSpeed { get; set; } = "R4.5";
        public string Min2WindDir { get; set; } = "90";
        public string Min2WindSpeed { get; set; } = "0.2f";
        public string Max2WindDir { get; set; } = "90";
        public string Max2WindSpeed { get; set; } = "0.2f";
        public string Avg2WindDir { get; set; } = "90";
        public string Avg2WindSpeed { get; set; } = "0.2f";
    }
}
