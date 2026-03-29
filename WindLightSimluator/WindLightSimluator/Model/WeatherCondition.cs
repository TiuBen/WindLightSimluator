using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindLightSimluator.Model
{
    public class WeatherCondition
    {

        public string CloudFirstLayer { get; set; } = "NCD";

        public double Temperature { get; set; } = 25.4;

        public double SurfaceTemperature { get; set; }=22.2;

        public double Duepoint { get; set; } = 1.4;


        public int VVIS { get; set; } 


        public double  Rain1h { get; set; } = 0.0;

        public double RelativeHumidity { get; set; } = 32;

        public double Rain24h { get; set; } = 0.0;

        public double QFE { get; set; } = 1017.2;


        public string  Status { get; set; } = "Dry";

    }
}
