using System.Runtime.InteropServices;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public string WriteCell(object sheet, int row, int column, string value)
    {
        ((workbook.Worksheets[sheet]).Range[row, column]).Value2 = value;
        return "Edited cell on row " + row + " and column " + column + " to " + value + ".";
    }
    
    public string WriteCellModern(object sheet, int row, int column, string value)
    {
        var worksheet = workbook.Worksheets[sheet];
        var range = worksheet.Cells[row, column];
        range.Value2 = value;

        Marshal.ReleaseComObject(range);
        Marshal.ReleaseComObject(worksheet);

        return $"Edited cell on row {row} and column {column} to {value}.";
    }

    
}