using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib.Model
{

    public class UnitHistoricalState
    {

        public UnitHistoricalState()
        {
            Measure = new UnitHistoricalMeasure();
            Status = new UnitStates();
        }
        public UnitHistoricalMeasure Measure { get; set; }
        public UnitStates Status { get; set; }

    
    }
    

}
