using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindLightSimluator.ViewModels.vm;
using System.Collections.ObjectModel;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public RunwayViewModel WestRunway { get; }
        public RunwayViewModel EastRunway { get; }


        public MainWindowViewModel()
        {
            WestRunway = new RunwayViewModel();
            WestRunway.RunwayStart.Status.RunwayNumber = "01L";
            WestRunway.RunwayMiddle.Status.RunwayNumber = "MID1";
            WestRunway.RunwayEnd.Status.RunwayNumber = "19R";
            EastRunway = new RunwayViewModel();
            WestRunway.RunwayStart.Status.RunwayNumber = "01R";
            WestRunway.RunwayMiddle.Status.RunwayNumber = "MID1";
            WestRunway.RunwayEnd.Status.RunwayNumber = "19L";
        }


    }
}
