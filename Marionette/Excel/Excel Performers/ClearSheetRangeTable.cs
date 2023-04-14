using System.Runtime.InteropServices;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public void ClearSheetRangeTable(object sheet, string range = "", string tableName = "")
    {
        var worksheet = workbook.Worksheets[sheet];

        if (!string.IsNullOrEmpty(tableName))
        {
            var targetTable = worksheet.ListObjects[tableName];
            targetTable.DataBodyRange.ClearContents();
            Marshal.ReleaseComObject(targetTable);
        }
        else if (!string.IsNullOrEmpty(range))
        {
            var targetRange = worksheet.Range[range];
            targetRange.ClearContents();
            Marshal.ReleaseComObject(targetRange);
        }
        else
        {
            worksheet.Cells.Clear();
        }

        Marshal.ReleaseComObject(worksheet);
    }
}