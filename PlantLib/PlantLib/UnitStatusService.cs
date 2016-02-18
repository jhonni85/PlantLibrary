using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib.Model
{
    public class UnitStatusService
    {
        int OffHours { get; set; }

        int IgnitionRampHours { get; set; }
        int ShutDownRampHours { get; set; }

        int DataAnomalyHours { get; set; }
        int RunningHurs { get; set; }
        int Cewe { get; set; }
        UnitStates Status { get; set; }
        List<UnitHistoricalState> history { get; set; }


        public UnitStatusService()
        {
            OffHours = 0;
            Cewe = 0;
            IgnitionRampHours = 0;
            ShutDownRampHours = 0;
            RunningHurs = 0;
            DataAnomalyHours = 0;
            Status = UnitStates.off;
            history = new List<UnitHistoricalState>();
        }

        public List<UnitHistoricalState> GetStatus()
        {
            return history;
        }
        public void ApplyNewData(UnitHistoricalMeasure measure)
        {
 
            // if plant is off 
            if (UnitStates.off == Status)
            {
                if (measure.Cewe > 0)
                {
                    if (OffHours < 8)
                        Status = UnitStates.ignitionRampHot;
                    if (OffHours >= 8 && OffHours < 48)
                        Status = UnitStates.ignitionRampWorm;
                    if (OffHours >= 48)
                        Status = UnitStates.ignitionRampCold;
                     OffHours = 0;
                }
                if(measure.Cewe == 0)
                {
                    OffHours = OffHours + 1;
                }
            }
            else if (UnitStates.ignitionRampCold == Status || 
                UnitStates.ignitionRampHot == Status || 
                UnitStates.ignitionRampWorm == Status)
            {
                if (measure.Cewe < measure.Pmin)
                {
                    IgnitionRampHours = IgnitionRampHours + 1;
                }
                if (measure.Cewe >= measure.Pmin)
                {
                    Status = UnitStates.running;
                    IgnitionRampHours = 0;
                }
                if (measure.Cewe == 0)
                {
                    _turnOff(measure);
                    IgnitionRampHours = 0;
                }
            }
            else if (UnitStates.running == Status)
            { 
                if (measure.Cewe >= measure.Pmin)
                {
                    RunningHurs = RunningHurs + 1;
                }
                if (measure.Cewe < measure.Pmin-10)
                {
                    Status = UnitStates.shutdownRamp;
                    RunningHurs = 0;
                }
                if (measure.Cewe == 0 )
                {
                    _turnOff(measure);
                    RunningHurs = 0;
                }
            }
            else if (UnitStates.shutdownRamp == Status)
            {
                if (measure.Cewe > 0 && measure.Cewe < measure.Pmin)
                {
                    ShutDownRampHours = ShutDownRampHours + 1; ;
                }
                if (measure.Cewe == 0)
                {
                    _turnOff(measure);
                   ShutDownRampHours = 0;
                }
                if (measure.Cewe >= measure.Pmin)
                {
                    Status = UnitStates.running;
                    ShutDownRampHours = 0 ;
                }
            }
           
            history.Add(new UnitHistoricalState () { Measure = measure, Status = Status });
        }
        void _turnOff(UnitHistoricalMeasure measure)
        {
            if(IgnitionRampHours>0 && 
                (
                UnitStates.ignitionRampCold == Status ||
                UnitStates.ignitionRampHot == Status ||
                UnitStates.ignitionRampWorm == Status)
                )
            {
                var start= measure.Date.AddHours(-IgnitionRampHours);

                var data = history.Where(x => x.Measure.Date > start && x.Measure.Date <= measure.Date);
                foreach (var item in data)
                {
                    item.Status = UnitStates.ignitionTest;
                }

            }
            Status = UnitStates.off;
         }
    }
}
