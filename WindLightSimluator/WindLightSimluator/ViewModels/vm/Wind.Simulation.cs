using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels.vm
{
    public partial class WindVM : ViewModelBase
    {

        // --- 新增：角度索引 (0-35) ---
        public int AngleIndex
        {

            get {
                double dir = _windDir;

                // 关键：平移5度做区间归属
                dir = (dir + 5) % 360;

                return (int)(dir / 10);
            }
        }

        public double AngleHeading
        {
            get {
                double roundedDir = Math.Round(_windDir / 10.0) * 10;
                int index = (int)((roundedDir + 5) / 10); if (index >= 36) index = 0;
                return index * 10;
            }
        }






    }
}
