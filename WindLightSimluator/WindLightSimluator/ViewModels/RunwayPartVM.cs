using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.ViewModels.vm;

namespace WindLightSimluator.ViewModels
{
    public class RunwayPartVM
    {
        public RunwayStatusVM status { get; set; }
        public WindVM wind { get; set; }
        public WindStatisticsVM statistics { get; set; }
        public RvrVisVM rvrVis { get; set; }
        public WeatherConditionVM weather { get; set; }
    }
}
