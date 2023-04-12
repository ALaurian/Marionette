using System.Runtime.InteropServices;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public object GetRangeValue(object sheet, string range)
    {
        return (workbook.Worksheets[sheet]).Range[range].Value2;
    }
    
    public object GetRangeValueModern(object sheet, string range)
    {
        var worksheet = workbook.Worksheets[sheet];
        var rangeValues = worksheet.Range[range].Value2;

        Marshal.ReleaseComObject(worksheet);

        return rangeValues;
    }

}