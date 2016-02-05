using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib.PlantDataServices.Model
{
    public class TimeSerie
    {
        public DateTime D { get; set; }
        public float V { get; set; }
        public int H { get; set; }
        public DateTime DQ { get; set; }

    }
}
