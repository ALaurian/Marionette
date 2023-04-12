using System.Runtime.InteropServices;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public string ReadCellFormula(object sheet, int row, int column)
    {
        var worksheet = workbook.Worksheets[sheet];
        var cell = worksheet.Cells[row, column];

        var cellFormula = cell.Formula;

        Marshal.ReleaseComObject(cell);
        Marshal.ReleaseComObject(worksheet);

        return cellFormula;
    }
}