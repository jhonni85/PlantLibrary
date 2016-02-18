using PlantLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib
{
    public interface IPlantService
    {
        Plant GetPlant(Plants plant);

        IEnumerable<UnitHistoricalMeasure> GetUnitHistoricalMeasure(Plants PlantName, int ModuleNumber);
        IEnumerable<PlantHistoricalMeasure> GetPlantHistoricalMeasure(Plants PlantName);
        IEnumerable<UnitHistoricalState> GetUnitStatus(Plants PlantName, int ModuleNumber);

        RegressionParameters GasConsumptionRegression(Plant plant, int ModuleNumber, UnitStates[] s);
    }
}
