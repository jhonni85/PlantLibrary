using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PlantLib.Model;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLib.ExcelServices
{
    public class ExcelService
    {
        HSSFWorkbook wb;
        public ExcelService()
        {
            _loadTemplateWorkbook(@".\ExcelServices\Template\Previsione Gas Centrali.xls");
        }
        private HSSFWorkbook _loadTemplateWorkbook(string path)
        {
       
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                wb = new HSSFWorkbook(fs);
            }
            return wb;
        }
        public void WriteGasRegression(Plants PlantName,int ModuleNumber ,IEnumerable<RegressionParameters> RP)
        {
            var sh = wb.GetSheet(PlantName + "_" + ModuleNumber);
            IRow header = sh.GetRow(0);
            header
                .CreateCell(13)
                .SetCellValue("Status");
            header
                .CreateCell(14)
                .SetCellValue("Intercept");  
            header
                .CreateCell(15)
                .SetCellValue("Cewe");
            header
                .CreateCell(16)
                .SetCellValue("Humidity");
            header
                .CreateCell(17)
                .SetCellValue("Pressure");
            header
                .CreateCell(18)
                .SetCellValue("Temperature");
 

            int i = 1;
            foreach (var item in RP)
            { 
                foreach(var p in item.UnitState)
                {
                    IRow row = sh.GetRow(i);
                    row
                        .CreateCell(13)
                        .SetCellValue(p.ToString());
                    row
                        .CreateCell(14)
                        .SetCellValue(item.intercept);
                    row
                        .CreateCell(15)
                        .SetCellValue(item.Cewe);
                    row
                        .CreateCell(16)
                        .SetCellValue(item.Humidity);
                    row
                        .CreateCell(17)
                        .SetCellValue(item.Pressure);
                    row
                        .CreateCell(18)
                        .SetCellValue(item.Temperature);
                  
                    i++;
                }
               
            }
        }
        private void _fillsheetWithUnitValue( Plant p, Unit unit, ISheet sh)
        {
            IRow header = sh.CreateRow(0);
            header
                .CreateCell(0)
                .SetCellValue("OtherModuleRunning");
            header
                .CreateCell(1)
                .SetCellValue("Date");
            header
                .CreateCell(2)
                .SetCellValue("Cewe");
            header
                .CreateCell(3)
                .SetCellValue("Pmin");
            header
                .CreateCell(4)
                .SetCellValue("Pmax");
            header
                .CreateCell(5)
                .SetCellValue("BurnedGas");
            header
                .CreateCell(6)
                .SetCellValue("Humidity");
            header
                .CreateCell(7)
                .SetCellValue("Pressure");
            header
                .CreateCell(8)
                .SetCellValue("Temperature");
            header
                .CreateCell(9)
                .SetCellValue("Pcs");
            header
                .CreateCell(10)
                .SetCellValue("Status");


            var output = from x in unit.UnitHistoricalData
                         join y in p.PlantHistoricalData on x.Measure.Date equals y.Date
                         select new { x.Measure.Date, x.Measure.Cewe, x.Measure.Pmin, x.Measure.Pmax, x.Measure.BurnedGas, y.Humidity, y.Pressure, y.Temperature,  x.Status,y.Pcs,x.OtherModuleRunning };

            int i = 1;
            foreach (var item in output)
            {
                IRow row = sh.CreateRow(i);
                row
                    .CreateCell(0)
                    .SetCellValue(item.OtherModuleRunning);
                row
                    .CreateCell(1)
                    .SetCellValue(item.Date);
                row
                    .CreateCell(2)
                    .SetCellValue(item.Cewe);
                row
                    .CreateCell(3)
                    .SetCellValue(item.Pmin);
                row
                    .CreateCell(4)
                    .SetCellValue(item.Pmax);
                row
                    .CreateCell(5)
                    .SetCellValue(item.BurnedGas);
                row
                    .CreateCell(6)
                    .SetCellValue(item.Humidity);
                row
                    .CreateCell(7)
                    .SetCellValue(item.Pressure);
                row
                    .CreateCell(8)
                    .SetCellValue(item.Temperature);
                row
                    .CreateCell(9)
                    .SetCellValue(item.Pcs);
                row
                    .CreateCell(10)
                    .SetCellValue(item.Status.ToString());
         
                i++;
            }
        }
        public  void CreatePlantSheets(Plant p)
        {
 
  

            var sh1=wb.GetSheet(p.Name + "_"+ p.Unit1.ModuleNumber);
            _fillsheetWithUnitValue(p,p.Unit1, sh1);

     
            var sh2 = wb.GetSheet(p.Name + "_" + p.Unit2.ModuleNumber);
            _fillsheetWithUnitValue(p,p.Unit2, sh2);
 
        }
        public void SaveWorkbook(string destPath)
        {
            using (FileStream fs = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                wb.Write(fs);
            }
        }
    }
}
