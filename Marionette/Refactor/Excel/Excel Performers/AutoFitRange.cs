using System.Runtime.InteropServices;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public void AutoFitRange(object sheet, string range)
    {
        var worksheet = workbook.Worksheets[sheet];
        var targetRange = worksheet.Range[range];
        targetRange.EntireColumn.AutoFit();
        targetRange.EntireRow.AutoFit();

        Marshal.ReleaseComObject(targetRange);
        Marshal.ReleaseComObject(worksheet);
    }
}