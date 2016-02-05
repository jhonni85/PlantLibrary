using PlantLib.Model;
using PlantLib.PlantDataServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib.PlantDataServices
{
    public interface IPlantRepository
    {
        IEnumerable<TimeSerie> GetCewe(Plants Plant, int Unit);
        PlantRepositoryStaticInfo GetConfig(Plants Plant);
 

    }
}
