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
        

        public bool GetUnitStatus(Unit u)
        {
            var cewe = _plantRepo.GetCewe(u.PlantName, u.ModuleNumber);
            var pmin = _plantRepo.GetPmin(u.PlantName, u.ModuleNumber);
            var pmax = _plantRepo.GetPmax(u.PlantName, u.ModuleNumber);

            var result = from x in cewe
                         join y in pmin on new { x.D, x.H } equals new { y.D, y.H }
                         join z in pmax on new { x.D, x.H } equals new { z.D, z.H }
                         select new { D= x.D, H = x.H,Cewe = x.V , Pmin=y.V, Pmax=z.V};

            var orderedResult = result.Where(x=>x.D >= new DateTime(2013,01,01)).OrderBy(x => x.D).ThenBy(x => x.H).ToList();
            for (int i = 0; i < orderedResult.Count; i++)
            {
                var cane = orderedResult[i];
            }
            
            return true;
        }


        public Plant GetPlant(Plants plant)
        {
    
            var p = new Plant();

            p.Name = Plants.Rizziconi;
            p.Unit1 = new Unit();
            p.Unit1.ModuleNumber = 1;
            p.Unit1.PlantName = Plants.Rizziconi;
            p.Unit2 = new Unit();
            p.Unit2.ModuleNumber = 2;
            p.Unit2.PlantName = Plants.Rizziconi;

            return p;
        }

        public bool GetPlantStatus(Unit u)
        {
            throw new NotImplementedException();
        }

    }
}
