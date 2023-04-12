using System.Runtime.InteropServices;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public object ReadCellValue(object sheet, int row, int column)
    {
        return ((workbook.Worksheets[sheet]).Cells[row, column] as Range).Value2;
    }
    
    public object ReadCellValueModern(object sheet, int row, int column)
    {
        var worksheet = workbook.Worksheets[sheet];
        var cell = worksheet.Cells[row, column];
        var cellValue = cell.Value2;

        Marshal.ReleaseComObject(cell);
        Marshal.ReleaseComObject(worksheet);

        return cellValue;
    }

}