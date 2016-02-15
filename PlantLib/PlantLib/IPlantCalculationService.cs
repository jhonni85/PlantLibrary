using PlantLib.Model;

namespace PlantLib
{
    public interface IPlantCalculationService
    {
        RegressionParameters GasConsumptionRegression(Plant plant, int ModuleNumber, UnitStates[] s);
    }
}