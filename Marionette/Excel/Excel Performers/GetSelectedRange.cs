using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public Range GetSelectedRange()
    {
        Worksheet activeWorksheet = workbook.ActiveSheet;
        var selectedRange = activeWorksheet.Application.Selection as Range;

        if (selectedRange == null)
        {
            throw new InvalidOperationException("No range is currently selected.");
        }

        return selectedRange;
    }
}