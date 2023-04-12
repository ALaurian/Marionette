using System.Runtime.InteropServices;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public void Filter(object sheet, string range, string criteria)
    {
        var worksheet = workbook.Worksheets[sheet];
        var targetRange = worksheet.Range[range];
        targetRange.AutoFilter(1, criteria);

        Marshal.ReleaseComObject(targetRange);
        Marshal.ReleaseComObject(worksheet);
    }
    
    public void Filter(object sheet, string range, params string[] multipleCriteria)
    {
        var worksheet = workbook.Worksheets[sheet];
        var targetRange = worksheet.Range[range];

        for (int i = 0; i < multipleCriteria.Length; i++)
        {
            targetRange.AutoFilter(i + 1, multipleCriteria[i]);
        }

        Marshal.ReleaseComObject(targetRange);
        Marshal.ReleaseComObject(worksheet);
    }
}