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
        IEnumerable<TimeSerie> GetTemperature(Plants Plant);
        IEnumerable<TimeSerie> GetPressure(Plants Plant);
        IEnumerable<TimeSerie> GetHumdity(Plants Plant);
        PlantRepositoryStaticInfo GetConfig(Plants Plant);
        IEnumerable<TimeSerie> GetPmax(Plants Plant, int Unit);
        IEnumerable<TimeSerie> GetPmin(Plants Plant, int Unit);

    }
}
