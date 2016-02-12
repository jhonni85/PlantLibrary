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
using Oracle.ManagedDataAccess.Client;

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
        private IEnumerable<TimeSerie> _getXDMPot( string UnitName,string CurveType , string CurveSubType)
        {
            using (var connection = new OracleConnection(_config.OracleConnectionString))
            {
                connection.Open();
           
                var query = @"
SELECT
 
	INTERVAL_DATE AS D,
	trunc(to_number((quarto-1)/4 +1)) AS H,
	avg(to_number(Valore*-1)) AS V,
    INTERVAL_DATE AS DQ
FROM (

  SELECT 
    c.unit,
    c.curve_subtype,
    ci.interval_Date,
    SUM(VALUE_001) as value_001,SUM(VALUE_002) as value_002,SUM(VALUE_003) as value_003,SUM(VALUE_004) as value_004,SUM(VALUE_005) as value_005,
    SUM(VALUE_006) as value_006,SUM(VALUE_007) as value_007,SUM(VALUE_008) as value_008,SUM(VALUE_009) as value_009,SUM(VALUE_010) as value_010,
    SUM(VALUE_011) as value_011,SUM(VALUE_012) as value_012,SUM(VALUE_013) as value_013,SUM(VALUE_014) as value_014,SUM(VALUE_015) as value_015,
    SUM(VALUE_016) as value_016,SUM(VALUE_017) as value_017,SUM(VALUE_018) as value_018,SUM(VALUE_019) as value_019,SUM(VALUE_020) as value_020,
    SUM(VALUE_021) as value_021,SUM(VALUE_022) as value_022,SUM(VALUE_023) as value_023,SUM(VALUE_024) as value_024,SUM(VALUE_025) as value_025,
    SUM(VALUE_026) as value_026,SUM(VALUE_027) as value_027,SUM(VALUE_028) as value_028,SUM(VALUE_029) as value_029,SUM(VALUE_030) as value_030,
    SUM(VALUE_031) as value_031,SUM(VALUE_032) as value_032,SUM(VALUE_033) as value_033,SUM(VALUE_034) as value_034,SUM(VALUE_035) as value_035,
    SUM(VALUE_036) as value_036,SUM(VALUE_037) as value_037,SUM(VALUE_038) as value_038,SUM(VALUE_039) as value_039,SUM(VALUE_040) as value_040,
    SUM(VALUE_041) as value_041,SUM(VALUE_042) as value_042,SUM(VALUE_043) as value_043,SUM(VALUE_044) as value_044,SUM(VALUE_045) as value_045,
    SUM(VALUE_046) as value_046,SUM(VALUE_047) as value_047,SUM(VALUE_048) as value_048,SUM(VALUE_049) as value_049,SUM(VALUE_050) as value_050,
    SUM(VALUE_051) as value_051,SUM(VALUE_052) as value_052,SUM(VALUE_053) as value_053,SUM(VALUE_054) as value_054,SUM(VALUE_055) as value_055,
    SUM(VALUE_056) as value_056,SUM(VALUE_057) as value_057,SUM(VALUE_058) as value_058,SUM(VALUE_059) as value_059,SUM(VALUE_060) as value_060,
    SUM(VALUE_061) as value_061,SUM(VALUE_062) as value_062,SUM(VALUE_063) as value_063,SUM(VALUE_064) as value_064,SUM(VALUE_065) as value_065,
    SUM(VALUE_066) as value_066,SUM(VALUE_067) as value_067,SUM(VALUE_068) as value_068,SUM(VALUE_069) as value_069,SUM(VALUE_070) as value_070,
    SUM(VALUE_071) as value_071,SUM(VALUE_072) as value_072,SUM(VALUE_073) as value_073,SUM(VALUE_074) as value_074,SUM(VALUE_075) as value_075,
    SUM(VALUE_076) as value_076,SUM(VALUE_077) as value_077,SUM(VALUE_078) as value_078,SUM(VALUE_079) as value_079,SUM(VALUE_080) as value_080,
    SUM(VALUE_081) as value_081,SUM(VALUE_082) as value_082,SUM(VALUE_083) as value_083,SUM(VALUE_084) as value_084,SUM(VALUE_085) as value_085,
    SUM(VALUE_086) as value_086,SUM(VALUE_087) as value_087,SUM(VALUE_088) as value_088,SUM(VALUE_089) as value_089,SUM(VALUE_090) as value_090,
    SUM(VALUE_091) as value_091,SUM(VALUE_092) as value_092,SUM(VALUE_093) as value_093,SUM(VALUE_094) as value_094,SUM(VALUE_095) as value_095,
    SUM(VALUE_096) as value_096,SUM(VALUE_097) as value_097,SUM(VALUE_098) as value_098,SUM(VALUE_099) as value_099,SUM(VALUE_100) as value_100
  FROM curve c inner join curve_interval_15 ci on c.id_curve=ci.id_curve
  WHERE c.status ='A'       
    AND c.deleted = 0

    AND CURVE_TYPE= :CURVE_TYPE
    AND unit= :UNIT_NAME
    AND curve_subtype = :CURVE_SUBTYPE

   GROUP BY UNIT,  CURVE_SUBTYPE,INTERVAL_DATE
) p
unPIVOT ( Valore FOR Quarto IN (
 value_001 as '1',  value_002 as '2',  value_003 as '3',  value_004 as '4',  value_005 as '5', 
 value_006 as '6',  value_007 as '7',  value_008 as '8',  value_009 as '9',  value_010 as '10', 
 value_011 as '11', value_012 as '12', value_013 as '13', value_014 as '14', value_015 as '15', 
 value_016 as '16', value_017 as '17', value_018 as '18', value_019 as '19', value_020 as '20', 
 value_021 as '21', value_022 as '22', value_023 as '23', value_024 as '24', value_025 as '25', 
 value_026 as '26', value_027 as '27', value_028 as '28', value_029 as '29', value_030 as '30', 
 value_031 as '31', value_032 as '32', value_033 as '33', value_034 as '34', value_035 as '35', 
 value_036 as '36', value_037 as '37', value_038 as '38', value_039 as '39', value_040 as '40', 
 value_041 as '41', value_042 as '42', value_043 as '43', value_044 as '44', value_045 as '45', 
 value_046 as '46', value_047 as '47', value_048 as '48', value_049 as '49', value_050 as '50', 
 value_051 as '51', value_052 as '52', value_053 as '53', value_054 as '54', value_055 as '55', 
 value_056 as '56', value_057 as '57', value_058 as '58', value_059 as '59', value_060 as '60', 
 value_061 as '61', value_062 as '62', value_063 as '63', value_064 as '64', value_065 as '65', 
 value_066 as '66', value_067 as '67', value_068 as '68', value_069 as '69', value_070 as '70', 
 value_071 as '71', value_072 as '72', value_073 as '73', value_074 as '74', value_075 as '75', 
 value_076 as '76', value_077 as '77', value_078 as '78', value_079 as '79', value_080 as '80', 
 value_081 as '81', value_082 as '82', value_083 as '83', value_084 as '84', value_085 as '85', 
 value_086 as '86', value_087 as '87', value_088 as '88', value_089 as '89', value_090 as '90', 
 value_091 as '91', value_092 as '92', value_093 as '93', value_094 as '94', value_095 as '95', 
 value_096 as '96', value_097 as '97', value_098 as '98', value_099 as '99', value_100 as '100'
 )) d
 group by unit, curve_subtype, INTERVAL_DATE, trunc(to_number((quarto-1)/4 +1))
";
                var command = new CommandDefinition(query, new { CURVE_TYPE = CurveType , UNIT_NAME = UnitName , CURVE_SUBTYPE = CurveSubType});

                IEnumerable<TimeSerie> res = connection.Query<TimeSerie>(command);
                connection.Close();
                connection.Dispose();
                return res;
                
                    
            }

        }
        public IEnumerable<TimeSerie> GetPmin(Plants Plant, int Unit)
        {
            var info = GetConfig(Plant).Units.Where(x => x.UnitID == Unit).Single();

            return _getXDMPot(info.XDM_Unit, info.XDM_PMCurveType, info.XDM_PMinCurveSubType);

        }
        public IEnumerable<TimeSerie> GetPmax(Plants Plant, int Unit)
        {
            var info = GetConfig(Plant).Units.Where(x => x.UnitID == Unit).Single();

            return _getXDMPot(info.XDM_Unit,info.XDM_PMCurveType,info.XDM_PMaxCurveSubType);

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

      
 
