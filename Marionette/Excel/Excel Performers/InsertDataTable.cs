using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using DataTable = System.Data.DataTable;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public void InsertDataTable(object sheet, int row, DataTable dataTable, bool deleteEntireSheet = true,
        bool dataTableHeader = true)
    {
        var dt_rows = dataTable.Rows.Cast<DataRow>();

        foreach (var dtrow in dt_rows)
        {
            dtrow.ItemArray = dtrow.ItemArray.Select(x =>
            {
                if (x.GetType() == typeof(DBNull))
                {
                    x = "";
                }

                return x;
            }).ToArray();
        }


        (workbook.Worksheets[sheet]).Activate();
        //Delete all cells
        if (deleteEntireSheet)
        {
            (workbook.Worksheets[sheet]).Cells.Clear();
        }

        var columns = dataTable.Columns.Cast<DataColumn>();
        if (dataTableHeader)
        {
            var columnNames = columns.Select(column => column.ColumnName).ToArray();
            var joinedColumnNames = Strings.Join(columnNames, "	");

            Clipboard.SetText(joinedColumnNames);
            workbook.Worksheets[sheet].Cells[row, 1].Select();
            workbook.Worksheets[sheet].Paste();
            row++;
        }

        var rows = dataTable.Rows.Cast<DataRow>();
        var itemArrayJoiner = rows.Select(r => r.ItemArray).Aggregate("",
            (current, itemArray_original) => current +
                                             (Strings.Join(
                                                 itemArray_original.Cast<string>()
                                                     .Select(item =>
                                                         item.ReplaceLineEndings("")
                                                             .Replace("\t", ""))
                                                     .ToArray(), "	") + "\n"));
        Clipboard.SetText(itemArrayJoiner);
        workbook.Worksheets[sheet].Cells[row, 1].Select();
        workbook.Worksheets[sheet].Paste();
    }
    
    public void InsertDataTableModern(object sheet, DataTable dataTable, bool deleteEntireSheet = true,
        bool dataTableHeader = true, int startRow = 1, int startColumn = 1)
    {
        // Clear the existing data in the sheet
        if (deleteEntireSheet)
        {
            var worksheet = workbook.Worksheets[sheet];
            worksheet.UsedRange.ClearContents();
            Marshal.ReleaseComObject(worksheet);
        }

        // Copy the column headers to the sheet
        if (dataTableHeader)
        {
            Range headerRange = workbook.Worksheets[sheet].Cells[startRow, startColumn];
            headerRange.Resize[1, dataTable.Columns.Count].Value = dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
            Marshal.ReleaseComObject(headerRange);
        }

        // Copy the data to the sheet
        Range dataRange = workbook.Worksheets[sheet].Cells[startRow + 1, startColumn];
        dataRange.Resize[dataTable.Rows.Count, dataTable.Columns.Count].Value = dataTable.Rows.Cast<DataRow>().SelectMany(r => r.ItemArray).ToArray();

        // Release the COM objects
        Marshal.ReleaseComObject(dataRange);

    }

}