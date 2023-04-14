using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public bool AutoFill(object sheet, string range, int lastRow)
    {
        //split range by ":"
        var rangeArray = range.Split(':')[1];
        //replace all numbers in range
        var rangeArrayWithoutNumbers = Regex.Replace(rangeArray, @"\d", "");


        workbook.Worksheets[sheet].Range(range)
            .AutoFill(
                workbook.Worksheets[sheet].Range(range.Split(':')[0] + ":" + rangeArrayWithoutNumbers + lastRow),
                XlAutoFillType.xlFillCopy);
        return true;
    }
}