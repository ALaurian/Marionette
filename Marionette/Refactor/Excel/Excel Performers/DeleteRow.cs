using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public string DeleteRow(object sheet, int row, XlDeleteShiftDirection direction)
    {
        ((workbook.Worksheets[sheet]).Rows[row]).EntireRow.Delete(direction);
        return "Deleted row " + row + ".";
    }
    
    public string DeleteRowModern(object sheet, int row, XlDeleteShiftDirection direction)
    {
        var worksheet = workbook.Worksheets[sheet];
        var rowRange = worksheet.Rows[row];
        rowRange.Delete(direction);

        Marshal.ReleaseComObject(rowRange);
        Marshal.ReleaseComObject(worksheet);

        return $"Deleted row {row}.";
    }

}