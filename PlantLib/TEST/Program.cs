using PlantLib;
using PlantLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            var plantService = new PlantService();

            var plant = plantService.GetPlant(Plants.Rizziconi);
            
        }
    }
}
