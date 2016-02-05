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
        
        private void _rampaDetector(List<TimeSerie> cewe)
        {
            var orderedCewe = cewe.OrderBy(x => x.DQ);
            var zeroCounter = 0;
            for (int i = 0; i < cewe.Count(); i++)
            {
                if (cewe[i].V == 0)
                {
                    zeroCounter++;
                }
                else
                {
                    if(zeroCounter<7)
                        
                }
                
                


            }
        }
        
     
    }
}
