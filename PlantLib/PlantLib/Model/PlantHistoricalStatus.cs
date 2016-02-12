using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib.Model
{
    public class PlantHistoricalStatus
    {
        Plants PlantName;

        IEnumerable<UnitHistoricalTimeSeries> unit1;
    }

    public class UnitHistoricalTimeSeries
    {
        DateTime Date;
        int Hour;
        float Cewe;
        float Pmin;
        float pmax;
        UnitStatus Rampa;
    }
    
}
