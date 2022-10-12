using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CoreLayout.Helper
{

   
    public static class ExcelHelper
    {
        public static MemoryStream CreateExcel(DataTable dt)
        {
            DataTable table = dt; //(DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data), (typeof(DataTable)));
            var memory = new MemoryStream();
            using (var memoryStream = new MemoryStream()) //creating memoryStream
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Sheet1");

                List<String> columns = new List<string>();
                IRow row = excelSheet.CreateRow(0);
                int columnIndex = 0;

                foreach (DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);
                    row.CreateCell(columnIndex).SetCellValue(column.ColumnName);
                    columnIndex++;
                }

                int rowIndex = 1;
                foreach (DataRow dsrow in table.Rows)
                {
                    row = excelSheet.CreateRow(rowIndex);
                    int cellIndex = 0;
                    foreach (String col in columns)
                    {
                        row.CreateCell(cellIndex).SetCellValue(dsrow[col].ToString());
                        cellIndex++;
                    }

                    rowIndex++;
                }
                workbook.Write(memoryStream);
                return memory;
            }
           
            
        }
    }
}
