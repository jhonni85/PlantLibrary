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
                JsonPlantStaticInfoPath = @"C:\workspace\PlantLibrary\PlantLib\PlantLib\PlantDataServices\PlantDataServiceInfo.JSON"
            };

            container.RegisterSingleton<PlantRepositoryConfig>(connectorCfg);
            container.RegisterSingleton<IPlantRepository, PlantRepository>();

            var rep = container.GetInstance<IPlantRepository>();

            rep.GetCewe(PlantLib.Model.Plants.Calenia, 1);

 
        }
    }
}

