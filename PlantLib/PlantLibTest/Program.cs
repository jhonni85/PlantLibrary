using MathNet.Numerics;
using Oracle.ManagedDataAccess.Client;
using PlantLib; 
using PlantLib.ExcelServices;
using PlantLib.Model;
using PlantLib.PlantDataServices;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLibTest
{
    class Program
    {
        static void Main(string[] args)
        {

      
            //***********************************/

            var plantService = new PlantService();
            var excel = new ExcelService();

            var plant = plantService.GetPlant(Plants.Rizziconi);
            excel.CreatePlantSheets(plant);

            List<RegressionParameters> rp = new List<RegressionParameters>();

            rp.Add(plantService.GasConsumptionRegression(plant, 1, new UnitStates[] { UnitStates.running }));
            rp.Add(plantService.GasConsumptionRegression(plant, 1, new UnitStates[] { UnitStates.ignitionRampCold}));
            rp.Add(plantService.GasConsumptionRegression(plant, 1, new UnitStates[] { UnitStates.ignitionRampHot }));
            rp.Add(plantService.GasConsumptionRegression(plant, 1, new UnitStates[] { UnitStates.ignitionRampWorm }));
            excel.WriteGasRegression(plant.Name, 1, rp);
            rp.Clear();

            rp.Add(plantService.GasConsumptionRegression(plant, 2, new UnitStates[] { UnitStates.running }));
            rp.Add(plantService.GasConsumptionRegression(plant, 2, new UnitStates[] { UnitStates.ignitionRampCold }));
            rp.Add(plantService.GasConsumptionRegression(plant, 2, new UnitStates[] { UnitStates.ignitionRampHot }));
            rp.Add(plantService.GasConsumptionRegression(plant, 2, new UnitStates[] { UnitStates.ignitionRampWorm }));
            excel.WriteGasRegression(plant.Name, 2, rp);
            rp.Clear();
            
            plant = plantService.GetPlant(Plants.Calenia);
            excel.CreatePlantSheets(plant);
            rp.Add(plantService.GasConsumptionRegression(plant, 1, new UnitStates[] { UnitStates.running }));
            rp.Add(plantService.GasConsumptionRegression(plant, 1, new UnitStates[] { UnitStates.ignitionRampCold }));
            rp.Add(plantService.GasConsumptionRegression(plant, 1, new UnitStates[] { UnitStates.ignitionRampHot }));
            rp.Add(plantService.GasConsumptionRegression(plant, 1, new UnitStates[] { UnitStates.ignitionRampWorm }));
            excel.WriteGasRegression(plant.Name, 1, rp);
            rp.Clear();

            rp.Add(plantService.GasConsumptionRegression(plant, 2, new UnitStates[] { UnitStates.running }));
            rp.Add(plantService.GasConsumptionRegression(plant, 2, new UnitStates[] { UnitStates.ignitionRampCold }));
            rp.Add(plantService.GasConsumptionRegression(plant, 2, new UnitStates[] { UnitStates.ignitionRampHot }));
            rp.Add(plantService.GasConsumptionRegression(plant, 2, new UnitStates[] { UnitStates.ignitionRampWorm }));
            excel.WriteGasRegression(plant.Name, 2, rp);
            rp.Clear();
             

            excel.SaveWorkbook("C:\\temp\\test2.xls");

        }
    }
}

