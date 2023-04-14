using System.Runtime.InteropServices;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public string FillRange(object sheet, string range, string value)
    {
        (workbook.Worksheets[sheet]).Range[range].Value2 = value;
        return "Edited range " + range + " to " + value + ".";
    }
    
    public string FillRangeModern(object sheet, string range, string value)
    {
        var worksheet = workbook.Worksheets[sheet];
        var targetRange = worksheet.Range[range];
        targetRange.Value2 = value;

        Marshal.ReleaseComObject(targetRange);
        Marshal.ReleaseComObject(worksheet);

        return $"Edited range {range} to {value}.";
    }
}