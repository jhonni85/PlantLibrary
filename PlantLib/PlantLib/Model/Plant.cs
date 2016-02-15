using PlantLib.PlantDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib.Model
{
    public class Plant
    {
        public Plants Name { get; set; }
        public Unit Unit1 { get; set; }
        public Unit Unit2 { get; set; }

        public IEnumerable<PlantHistoricalMeasure> PlantHistoricalData { get; set; }
    }
}
