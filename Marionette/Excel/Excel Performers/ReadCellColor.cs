using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public int ReadCellColor(object sheet, int row, int column)
    {
        var worksheet = workbook.Worksheets[sheet];
        var cell = worksheet.Cells[row, column];

        var interiorColor = cell.Interior.Color;

        Marshal.ReleaseComObject(cell);
        Marshal.ReleaseComObject(worksheet);

        return interiorColor;
    }
}