using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindLightSimluator.Model
{
    public class EditableWeatherElement
    {
        public DateTime Time { get; set; }

        public double? WindDirection { get; set; }
        public double? WindSpeed { get; set; }
        public double? Temperature { get; set; }
        public double? QNH { get; set; }
        public double? QFE { get; set; }
    }
}
