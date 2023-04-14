using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public string DeleteCell(object sheet, int row, int column, XlDeleteShiftDirection direction)
    {
        ((workbook.Worksheets[sheet]).Cells[row, column]).Delete(direction);
        return "Deleted cell on row " + row + " and column " + column + ".";
    }
    
    public string DeleteCellModern(object sheet, int row, int column, XlDeleteShiftDirection direction)
    {
        var cell = workbook.Worksheets[sheet].Cells[row, column];
        cell.Delete(direction);

        Marshal.ReleaseComObject(cell);
        return $"Deleted cell on row {row} and column {column}.";
    }

}