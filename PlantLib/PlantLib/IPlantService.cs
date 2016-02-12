using PlantLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib
{
    public interface IPlantService
    {
        Plant GetPlant(Plants plant);
        bool GetPlantStatus(Unit u);

        bool GetUnitStatus(Unit u);

    }
}
