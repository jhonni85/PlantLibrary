using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib.Model
{

    public class PlantHistoricalMeasure
    {
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float Pressure { get; set; }
        public float FisGas { get; set; }
        public float Pcs { get; set; }

    }
    

}
