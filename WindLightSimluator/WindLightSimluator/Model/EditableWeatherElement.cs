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

        public double? WindDirection { get; set; } = 180;
        public double? WindSpeed { get; set; } = 1;
        public double? Temperature { get; set; } = 24;
        public double? QNH { get; set; } = 1013;
        public double? QFE { get; set; } = 1011;
    }
}
