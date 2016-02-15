using MathNet.Numerics;
using PlantLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib
{
    public class PlantCalculationService : IPlantCalculationService
    {
        public IPlantService _plantService { get; private set; }
        public PlantCalculationService(IPlantService plantSer)
        {
            _plantService = plantSer;
        }

      
        public RegressionParameters GasConsumptionRegression(Plant plant, int ModuleNumber,UnitStates[] s) {

            if (s==null)
            {
                throw new ArgumentNullException("Unitstates[] : cannot be null");
            }
            if (plant == null)
            {
                throw new ArgumentNullException("Unit : cannot be null");
            }

            var impianto = plant.PlantHistoricalData;
            List<UnitHistoricalState> modulo = new List<UnitHistoricalState>();
            if (ModuleNumber == 1)
                modulo = plant.Unit1.UnitHistoricalData.ToList();
            if (ModuleNumber == 2)
                modulo = plant.Unit2.UnitHistoricalData.ToList();

            List<UnitHistoricalState> stateData = new List<UnitHistoricalState>();
            foreach (var item in s)
            {
                stateData.AddRange(modulo.Where(x => (item == x.Status)).ToArray());
            }
 

            var data = (from x in stateData
                        join y in impianto on x.Measure.Date equals y.Date
                        select new double[] { x.Measure.Cewe, y.Humidity, y.Pressure, y.Temperature }).ToArray();
            var gas = (from x in stateData
                       join y in impianto on x.Measure.Date equals y.Date
                       select Convert.ToDouble(x.Measure.BurnedGas)).ToArray();

            double[] p = Fit.MultiDim(data, gas, intercept: true);

            var dataforoutput = (from x in stateData
                                 join y in impianto on x.Measure.Date equals y.Date
                                 select new double[] { x.Measure.Cewe, y.Humidity, y.Pressure, y.Temperature, x.Measure.BurnedGas }).ToArray();

            return new RegressionParameters() { intercept = p[0], Cewe = p[1], Humidity = p[2], Pressure = p[3], Temperature = p[4] };
        }
    }
}
