using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindLightSimluator.Model
{




    public class UI_WQT 
    {
        public DateTime Time { get; init; }
        public string Temperature { get; set; }
        public string Qnh { get; set; }
        public string CurrentWindSpeed { get; set; }
        public string CurrentWindDir { get; set; }
        public string Min2WindSpeed { get; set; }
        public string Max2WindSpeed { get; set; }
        public string Avg2WindSpeed { get; set; }
        public string Min2WindDir { get; set; }
        public string Max2WindDir { get; set; }
        public string Avg2WindDir { get; set; }
        public string Min2HeadWindSpeed { get; set; }
        public string Max2HeadWindSpeed { get; set; }
        public string Avg2HeadWindSpeed { get; set; }

        public string Min5WindDir { get; set; }
        public string Max5WindDir { get; set; }


        public Wind Avg2Wind { get; set; }
        public Wind Min2Wind { get; set; }
        public Wind Max2Wind { get; set; }
    }
}