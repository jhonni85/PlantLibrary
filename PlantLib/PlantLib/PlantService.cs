using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantLib.Model;
using PlantLib.PlantDataServices;
using PlantLib.PlantDataServices.Model;
using System.IO;
using SimpleInjector;
using PlantLib.ExcelServices;
using System.Configuration;

namespace PlantLib
{
    public class PlantService : IPlantService
    {
        private IPlantRepository _plantRepo;
        private Container container;
        private ExcelService _excel;
        private IPlantCalculationService _plantCalculationService;
        private void _setContainer( )
        {
            container = new Container();

            PlantRepositoryConfig connectorCfg = new PlantRepositoryConfig()
            {
                SqlConnectionString = ConfigurationManager.AppSettings["PFX11Connection"],
                OracleConnectionString = ConfigurationManager.AppSettings["XDM_MERCATI"],
                JsonPlantStaticInfoPath = @"C:\workspace\PlantLibrary\PlantLib\PlantLib\PlantDataServices\PlantDataServiceInfo.JSON"
            };

            container.RegisterSingleton<PlantRepositoryConfig>(connectorCfg);
            container.RegisterSingleton<IPlantRepository, PlantRepository>();

            container.RegisterSingleton<IPlantService, PlantService>();
            container.RegisterSingleton<IPlantCalculationService, PlantCalculationService>();
            //***********************************/

         
            _plantCalculationService = container.GetInstance<IPlantCalculationService>();
        }
        public RegressionParameters GasConsumptionRegression(Plant plant, int ModuleNumber, UnitStates[] s)
        {
            return _plantCalculationService.GasConsumptionRegression(plant, ModuleNumber, s);
        }
        public PlantService( )
        {
            _setContainer();
            _plantRepo = container.GetInstance<IPlantRepository>();
            _excel = new ExcelService();
            
        }


        
        public Plant GetPlant(Plants plant)
        {

            var p = new Plant();

            p.Name = plant;
            p.Unit1 = new Unit();
            p.Unit1.ModuleNumber = 1;
            p.Unit1.PlantName = plant;
            p.Unit2 = new Unit();
            p.Unit2.ModuleNumber = 2;
            p.Unit2.PlantName = plant;

            p.PlantHistoricalData = GetPlantHistoricalMeasure(p.Name);
            p.Unit1.UnitHistoricalData = GetUnitStatus(p.Name, p.Unit1.ModuleNumber);
            p.PlantHistoricalData = GetPlantHistoricalMeasure(p.Name);
            p.Unit2.UnitHistoricalData = GetUnitStatus(p.Name, p.Unit2.ModuleNumber);
            _setOtherModuleRunning(p);
            return p;
        }
        private void _setOtherModuleRunning(Plant plant)
        {
            var cewes = from x in plant.Unit1.UnitHistoricalData
                        join y in plant.Unit2.UnitHistoricalData on x.Measure.Date equals y.Measure.Date
                        select new { x.Measure.Date, Cewe1 = x.Measure.Cewe, Cewe2 = y.Measure.Cewe };

            plant.Unit1.UnitHistoricalData.ToList().ForEach(x => x.OtherModuleRunning = false);
            plant.Unit2.UnitHistoricalData.ToList().ForEach(x => x.OtherModuleRunning = false);

            foreach (var item  in cewes)
            {
                if(item.Cewe1> 0 && item.Cewe2>0)
                {
                    var unitstatus = plant.Unit1.UnitHistoricalData.Where(x => x.Measure.Date == item.Date).SingleOrDefault();
                    unitstatus.OtherModuleRunning = true;
                     unitstatus = plant.Unit2.UnitHistoricalData.Where(x => x.Measure.Date == item.Date).SingleOrDefault();
                    unitstatus.OtherModuleRunning = true;
                }
                else if ( item.Cewe2 > 0)
                {
                    var unitstatus = plant.Unit1.UnitHistoricalData.Where(x => x.Measure.Date == item.Date).SingleOrDefault();
                    unitstatus.OtherModuleRunning = true;
 
                }
                else if (item.Cewe1 > 0)
                {
                    var unitstatus = plant.Unit2.UnitHistoricalData.Where(x => x.Measure.Date == item.Date).SingleOrDefault();
                    unitstatus.OtherModuleRunning = true;

                }

            }
        }
        public IEnumerable<UnitHistoricalMeasure> GetUnitHistoricalMeasure(Plants PlantName  , int ModuleNumber)
        {
            var cewe = _plantRepo.GetCewe(PlantName, ModuleNumber);
            var pmin = _plantRepo.GetPmin(PlantName, ModuleNumber);
            var pmax = _plantRepo.GetPmax(PlantName, ModuleNumber);
            var gas = _plantRepo.GetBurnedGas(PlantName, ModuleNumber);
            var result = from x in cewe
                         join y in pmin on new { x.D, x.H } equals new { y.D, y.H }
                         join z in pmax on new { x.D, x.H } equals new { z.D, z.H }
                         join g in gas on new { x.D, x.H } equals new { g.D, g.H }
                         select new UnitHistoricalMeasure() { Date = x.DT, Hour = x.H, Cewe = x.V, Pmin = y.V, Pmax = z.V ,BurnedGas = g.V };
            return result;
        }

        public IEnumerable<PlantHistoricalMeasure> GetPlantHistoricalMeasure(Plants PlantName)
        {
            var pressure = _plantRepo.GetPressure(PlantName);
            var humidity = _plantRepo.GetHumdity(PlantName);
            var temperature= _plantRepo.GetTemperature(PlantName);
            var fisgasprincipal= _plantRepo.GetFisGasPrincipal(PlantName);
            var fisgasreserve= _plantRepo.GetFisGasReserve(PlantName);

            var result = from x in pressure
                         join y in humidity on new { x.D, x.H } equals new { y.D, y.H }
                         join z in temperature on new { x.D, x.H } equals new { z.D, z.H }
                         join fg in fisgasprincipal on new { x.D, x.H } equals new {fg.D, fg.H }
                         join fgr in fisgasreserve on new { x.D, x.H } equals new { fgr.D, fgr.H }
                         select new PlantHistoricalMeasure() { Date = x.DT, Hour = x.H, Humidity = y.V, Pressure = x.V, Temperature = z.V,FisGas = fg.V+fgr.V };
            return result;
        }

        public IEnumerable<UnitHistoricalState> GetUnitStatus(Plants PlantName, int ModuleNumber)
        {
            var UnitMeaseure = GetUnitHistoricalMeasure(PlantName,ModuleNumber).ToArray();
            var UnitState = new UnitStatusService();
            for (int i = 0; i < UnitMeaseure.Count(); i++)
            {  
               UnitState.ApplyNewData(UnitMeaseure[i]);
            }
            return UnitState.GetStatus();
        }
     
    }
}
