using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.vm;

namespace WindLightSimluator.ViewModels
{
    public class RunwayViewModel:ViewModelBase
    {
        public RunwayColumnViewModel RunwayStart { get; }
        public RunwayColumnViewModel RunwayMiddle { get; }
        public RunwayColumnViewModel RunwayEnd { get; }

      

        internal void OnColumnSelected(RunwayColumnViewModel selected)
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

            RunwayStart = new RunwayColumnViewModel(this, RunwayPart.Start);
            RunwayMiddle = new RunwayColumnViewModel(this, RunwayPart.Middle);
            RunwayEnd = new RunwayColumnViewModel(this, RunwayPart.End);

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
