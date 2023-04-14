using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public string DeleteRange(object sheet, string range, XlDeleteShiftDirection direction)
    {
        (workbook.Worksheets[sheet]).Range[range].Delete(direction);
        return "Deleted range " + range + ".";
    }
    
    public string DeleteRangeModern(object sheet, string range, XlDeleteShiftDirection direction)
    {
        var worksheet = workbook.Worksheets[sheet];
        var rangeToDelete = worksheet.Range[range];
        rangeToDelete.Delete(direction);

        Marshal.ReleaseComObject(rangeToDelete);
        Marshal.ReleaseComObject(worksheet);

        return $"Deleted range {range}.";
    }

}