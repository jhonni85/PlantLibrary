using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib.Model
{
    public class RegressionParameters
    {
        public double intercept { get; set; }
        public double Cewe { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public double Temperature { get; set; }
        public IEnumerable<UnitStates> UnitState { get; set; } 
        public Plants PlantName { get; set; }
        public int ModuleNumber{ get; set; }
    }

}
