using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantLib.PlantDataServices.Model;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using PlantLib.Model;
using Newtonsoft.Json.Linq;
using System.IO;

namespace PlantLib.PlantDataServices
{
    public class PlantRepository : IPlantRepository
    {
        private PlantRepositoryConfig _config;
        private List<PlantRepositoryStaticInfo> _plantInfo;
        public PlantRepository(PlantRepositoryConfig config)
        {
            _config = config;
            _plantInfo = new List<PlantRepositoryStaticInfo>();
            _initializePlantInfo();
        }
        private void _initializePlantInfo()
        {
            JArray plantInfo = new JArray();
            String configJson = File.ReadAllText(_config.JsonPlantStaticInfoPath);
            plantInfo = JArray.Parse(configJson);
            foreach (JToken json in plantInfo)
            {
                _plantInfo.Add(json.ToObject<PlantRepositoryStaticInfo>());
            }
        }
        public PlantRepositoryStaticInfo GetConfig(Plants Plant)
        {
            return _plantInfo.Where(x => x.Name == Plant).Single();
        }

        private IEnumerable<TimeSerie>  _getProfile(int ProfileID)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                connection.Open();

                var query = @"
                           Select  
                             profile_date as D
                            ,ora H 
                            ,avg(valore) V 
                            ,MIN(GiornoQ) as DQ
                    FROM PREVGAS_QUARTER_PROFILES_DETAILS_PIVOTED
                    where PROFILE_ID in  (@profileID)
					group by profile_date,ora
                    ORDER BY D,H ";

                IEnumerable<TimeSerie> res = connection.Query<TimeSerie>(query, new { profileID = ProfileID });
                return res;
            }

        }
        public IEnumerable<TimeSerie> GetCewe(Plants Plant, int Unit)
        {
            var info = GetConfig(Plant).Units.Where(x => x.UnitID == Unit).Single();

            return _getProfile(info.CeweID);

        }

        public IEnumerable<TimeSerie> GetTemperature(Plants Plant)
        {
            var info = GetConfig(Plant);
            return _getProfile(info.TemperatureID);
        }

        public IEnumerable<TimeSerie> GetPressure(Plants Plant)
        {
            var info = GetConfig(Plant);
            return _getProfile(info.PressureID);
        }

        public IEnumerable<TimeSerie> GetHumdity(Plants Plant)
        {
            var info = GetConfig(Plant);
            return _getProfile(info.HumidityID);
        }
    }
       
}

      
 
