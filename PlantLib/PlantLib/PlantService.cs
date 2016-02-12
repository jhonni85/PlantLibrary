using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantLib.Model;
using PlantLib.PlantDataServices;
using PlantLib.PlantDataServices.Model;

namespace PlantLib
{
    public class PlantService : IPlantService
    {
        private IPlantRepository _plantRepo;

        public PlantService(IPlantRepository plantRepo)
        {
            _plantRepo = plantRepo;
 
            
        }

     

        public bool IsRampa(Unit u , DateTime date)
        {
            var result = _plantRepo.GetCewe(u.PlantName, u.ModuleNumber);
            return true;
        }
        
         
        
     
    }
}
