using PlantLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib.PlantDataServices
{
    public class PlantRepositoryStaticInfo
    {
        public Plants Name { get; set; }
        public int PlantID { get; set; }
        public int HumidityID { get; set; }
        public int PressureID { get; set; }
        public int TemperatureID { get; set; }
        public int PcsID { get; set; }
        public int FisGasPrincipalID { get; set; }
        public int FisGasReserveID { get; set; }
        public PlantUnitDataServiceStaticInfo[] Units { get; set; }
    }
    public class PlantUnitDataServiceStaticInfo
    {
        public int UnitID;
        public int CeweID;
        
    }
}
