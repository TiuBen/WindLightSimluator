using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels
{
    public partial class AirportVM:ViewModelBase
    {
        public void start(  Dictionary<string, ObservableCollection<double>> fakeData)
        {
            // 参数验证
            if (fakeData == null)
                throw new ArgumentNullException(nameof(fakeData));

            //




        }


    }
}
