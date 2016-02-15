using MathNet.Numerics;
using Oracle.ManagedDataAccess.Client;
using PlantLib;
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

            var container = new Container();

            PlantRepositoryConfig connectorCfg = new PlantRepositoryConfig()
            {
                SqlConnectionString = ConfigurationManager.AppSettings["PFX11Connection"],
                OracleConnectionString = ConfigurationManager.AppSettings["XDM_MERCATI"],
                JsonPlantStaticInfoPath = @"C:\workspace\PlantLibrary\PlantLib\PlantLib\PlantDataServices\PlantDataServiceInfo.JSON"
            };

            container.RegisterSingleton<PlantRepositoryConfig>(connectorCfg);
            container.RegisterSingleton<IPlantRepository, PlantRepository>();
 
            container.RegisterSingleton<IPlantService, PlantService>();
            container.RegisterSingleton<IPlantCalculationService, PlantCalculationService>();


            //***********************************/

            var plantService = container.GetInstance<IPlantService>();
            var calculationService = container.GetInstance<IPlantCalculationService>();
             
            var riz = plantService.GetPlant(Plants.Rizziconi);

            calculationService.GasConsumptionRegression(riz,1, new UnitStates[] { UnitStates.running });
            calculationService.GasConsumptionRegression(riz,2, new UnitStates[] { UnitStates.running });

            var cale = plantService.GetPlant(Plants.Calenia);

            calculationService.GasConsumptionRegression(cale,1, new UnitStates[] { UnitStates.running });
            calculationService.GasConsumptionRegression(cale,2, new UnitStates[] { UnitStates.running });

            var unitStatus = riz.Unit1.UnitHistoricalData;
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\temp\UnitStatus.txt"))
            {
                   foreach (var item in unitStatus)
                {
                    file.WriteLine(string.Format("{0};{1};{2};{3};{4}", item.Measure.Date, item.Measure.Cewe, item.Measure.Pmin, item.Measure.Pmax, item.Status));
                }

            }

        }
    }
}

