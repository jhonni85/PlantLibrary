using System.Collections.Generic;

namespace PlantLib.Model
{
    public class Unit  
    {
        public Plants PlantName {get; set;}
        public int ModuleNumber { get; set; }
        public IEnumerable<UnitHistoricalState> UnitHistoricalData { get; set; }

    }
}