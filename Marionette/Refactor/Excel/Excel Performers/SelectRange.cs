using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public void SelectRange(object sheet, string range)
    {
        Worksheet targetSheet = workbook.Worksheets[sheet];
        Range targetRange = targetSheet.Range[range];

        if (targetRange == null)
        {
            throw new ArgumentException("Invalid range specified.");
        }

        targetRange.Select();
    }
}