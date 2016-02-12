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

            var rep = container.GetInstance<IPlantService>();


            var riz = rep.GetPlant(Plants.Rizziconi);

            var result = rep.GetUnitStatus(riz.Unit1);


        }
    }
}

