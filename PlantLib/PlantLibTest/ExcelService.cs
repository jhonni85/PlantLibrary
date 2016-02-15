using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PlantLib.Model;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantLibTest
{
    class ExcelService
    {

        void CreateExcel(Plant p)
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            var unit = p.Unit1;
            var sh=wb.CreateSheet(p.Name + "_"+ unit.ModuleNumber);
            int i = 0;
            foreach (var item in unit.UnitHistoricalData)
            {
                IRow row = sh.CreateRow(i);
                row
                    .CreateCell(1)
                    .SetCellValue(item.Measure.Date);
                row
                    .CreateCell(2)
                    .SetCellValue(item.Measure.Cewe);
                row
                    .CreateCell(3)
                    .SetCellValue(item.Measure.Pmin);
                row
                    .CreateCell(4)
                    .SetCellValue(item.Measure.Pmax);
                row
                    .CreateCell(5)
                    .SetCellValue(item.Measure.BurnedGas);
                i++;
            }
        

            }
            private void _saveWorkbook(IWorkbook wb, string destPath)
            {
                using (FileStream fs = new FileStream(destPath, FileMode.Create, FileAccess.Write))
                {
                    wb.Write(fs);
                }
            }
    }
}
