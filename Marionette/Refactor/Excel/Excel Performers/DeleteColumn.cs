using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public string DeleteColumn(object sheet, int column, XlDeleteShiftDirection direction)
    {
        ((workbook.Worksheets[sheet]).Columns[column]).EntireColumn.Delete(direction);
        return "Deleted column " + column + ".";
    }
    
    public string DeleteColumnModern(object sheet, int column, XlDeleteShiftDirection direction)
    {
        var worksheet = workbook.Worksheets[sheet];
        var columnRange = worksheet.Columns[column];
        columnRange.Delete(direction);

        Marshal.ReleaseComObject(columnRange);
        Marshal.ReleaseComObject(worksheet);

        return $"Deleted column {column}.";
    }

}