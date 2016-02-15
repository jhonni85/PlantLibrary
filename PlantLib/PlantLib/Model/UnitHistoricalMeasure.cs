using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib.Model
{

    public class UnitHistoricalMeasure
    {
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public float Cewe { get; set; }
        public float Pmin { get; set; }
        public float Pmax { get; set; }
        public float BurnedGas{ get; set; }
    }
    

}
