using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindLightSimluator.Model
{
    public class RvrVis
    {
        private int _rvr = 2000;
        public string RvrValue
        {
            get {
                if (_rvr >= 2000)
                {
                    return $"P2000";
                }
                else
                {
                    return _rvr.ToString();
                }

            }
            set {   // 支持int赋值（通过字符串）
                if (int.TryParse(value, out int intValue))
                {
                    _rvr = intValue >= 0 ? intValue : 0;
                }
                else if (value?.StartsWith("P", StringComparison.OrdinalIgnoreCase) == true)
                {
                    _rvr = 2000;
                }
                else
                {
                    _rvr = 0;
                }
            }
        }

        private int _vis = 2000;
        public int VisValue
        {
            get { return _vis; }
            set { _vis = value; }
        }

    }
}
