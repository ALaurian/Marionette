using System.Data;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace Marionette.Excel_Scope;

public partial class Excel
{
   
    public DataTable WriteDataTableFromExcel(object sheet, int headerAt = 1, bool headerless = false)
    {
        var ws = workbook.Worksheets[sheet];
        ws.Activate();

        var lastColumnWithValue = ws.Cells[headerAt, ws.Columns.Count].End(XlDirection.xlToLeft).Column;
        var lastRowWithValue = ws.Cells[ws.Rows.Count, 1].End(XlDirection.xlUp).Row;

        var newDataTable = new DataTable();

        if (!headerless)
        {
            var headerRange =
                (object[,])ws.Range[ws.Cells[headerAt, 1], ws.Cells[headerAt, lastColumnWithValue]].Value2;

            for (var i = 1; i <= headerRange.Length; i++)
            {
                string columnName;
                try
                {
                    columnName = headerRange[1, i].ToString();
                }
                catch (Exception)
                {
                    columnName = $"Blank{i}";
                }

                newDataTable.Columns.Add(columnName, typeof(string));
            }
        }
        else
        {
            for (var i = 1; i <= lastColumnWithValue; i++)
            {
                newDataTable.Columns.Add(GetExcelColumnName(i), typeof(string));
            }
        }

        var dataRange = (object[,])ws.Range[ws.Cells[headerAt + 1, 1], ws.Cells[lastRowWithValue, lastColumnWithValue]]
            .Value2;

        for (var i = 1; i <= dataRange.GetLength(0); i++)
        {
            var newRow = newDataTable.NewRow();

            for (var j = 1; j <= dataRange.GetLength(1); j++)
            {
                newRow[j - 1] = dataRange[i, j]?.ToString() ?? "";
            }

            newDataTable.Rows.Add(newRow);
        }

        return newDataTable;
    }

}