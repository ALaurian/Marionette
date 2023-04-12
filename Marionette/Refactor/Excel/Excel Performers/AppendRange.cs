using System.Runtime.InteropServices;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public void AppendRange(object sheet, int startRow, int startColumn, object[,] values)
    {
        var worksheet = workbook.Worksheets[sheet];
        var rowCount = values.GetLength(0);
        var columnCount = values.GetLength(1);

        var targetRange = worksheet.Range[worksheet.Cells[startRow, startColumn],
            worksheet.Cells[startRow + rowCount - 1, startColumn + columnCount - 1]];
        targetRange.Value2 = values;

        Marshal.ReleaseComObject(targetRange);
        Marshal.ReleaseComObject(worksheet);
    }
}