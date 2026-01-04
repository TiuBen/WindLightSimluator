using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.vm;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels
{
    public class RunwayViewModel:ViewModelBase
    {
        public RunwayPartViewModel RunwayStart { get; }
        public RunwayPartViewModel RunwayMiddle { get; }
        public RunwayPartViewModel RunwayEnd { get; }

      

        internal void OnColumnSelected(RunwayPartViewModel selected)
        {
            // 中间列不参与
            //if (!selected.IsSelectable)
            //    return;

            //RunwayStart.State = RunwayColumnState.Normal;
            //RunwayEnd.State = RunwayColumnState.Normal;


            //selected.State = RunwayColumnState.Selected;
        }
        public RunwayViewModel()
        {

            RunwayStart = new RunwayPartViewModel(this, RunwayPartEnum.Start);
            RunwayStart.HeadCrossWind = new HeadCrossWindViewModel(this);
            RunwayStart.Wind.RangeArcIndex = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            RunwayStart.Wind.DirectedArcIndex = 2;
            RunwayStart.Wind.IsActive = true;
            HeadCrossWindViewModel _testHeadCrossWind = new HeadCrossWindViewModel();
            RunwayStart.HeadCrossWind = _testHeadCrossWind;
            RunwayStart.RvrVis = new RvrVisViewModel(RunwayStart);


            RunwayMiddle = new RunwayPartViewModel(this, RunwayPartEnum.Middle);
            RunwayMiddle.HeadCrossWind = new HeadCrossWindViewModel(this);
            RunwayMiddle.Wind.RangeArcIndex = new HashSet<int>() { 0, 1, 2, 3, 10, 11, 12, 13, 14, 15 };
            RunwayMiddle.Wind.DirectedArcIndex = 2;
            RunwayMiddle.Wind.IsActive = false;
            HeadCrossWindViewModel _testHeadCrossWind2 = new HeadCrossWindViewModel();
            _testHeadCrossWind2.Avg2HeadWindSpeed = 2;
            RunwayMiddle.HeadCrossWind = _testHeadCrossWind2;


            RunwayEnd = new RunwayPartViewModel(this, RunwayPartEnum.End);
            RunwayEnd.HeadCrossWind = new HeadCrossWindViewModel(this);
            RunwayEnd.Wind.RangeArcIndex = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,  15 };
            RunwayEnd.Wind.DirectedArcIndex = 2;
            RunwayEnd.Wind.IsActive = false;
            HeadCrossWindViewModel _testHeadCrossWind3 = new HeadCrossWindViewModel();
            _testHeadCrossWind3.Max2WindSpeed = 4;
            RunwayEnd.HeadCrossWind = _testHeadCrossWind3;


            RunwayStart.IsActive = true;
            RunwayMiddle.IsActive = null;
            RunwayEnd.IsActive = false;

            //// 中间列固定 Normal
            //RunwayMiddle.State = RunwayColumnState.Normal;
            ////Columns[1].IsSelectable = false;
            //RunwayStart.State = RunwayColumnState.Selected;
            //RunwayEnd.State = RunwayColumnState.Disabled;
        }



    }
}
