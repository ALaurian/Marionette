using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public object Lookup(object sheet, string lookupValue, string lookupRange, string resultRange,
        XlFindLookIn lookIn = XlFindLookIn.xlValues,
        XlLookAt lookAt = XlLookAt.xlWhole,
        XlSearchOrder searchOrder = XlSearchOrder.xlByRows,
        XlSearchDirection searchDirection = XlSearchDirection.xlNext,
        bool matchCase = false)
    {
        var lookupWorksheet = workbook.Worksheets[sheet];
        var lookupColumn = lookupWorksheet.Range[lookupRange];
        var resultColumn = lookupWorksheet.Range[resultRange];
        var lookupCell = lookupWorksheet.Cells.Find(lookupValue, lookupColumn, LookIn: lookIn,
            LookAt: lookAt, SearchOrder: searchOrder,
            SearchDirection: searchDirection, MatchCase: matchCase);
        if (lookupCell == null)
        {
            throw new Exception($"Could not find the lookup value '{lookupValue}' in range '{lookupRange}'");
        }

        var rowIndex = lookupCell.Row - lookupColumn.Row + 1;
        var resultCell = resultColumn.Cells[rowIndex, 1];

        Marshal.ReleaseComObject(resultColumn);
        Marshal.ReleaseComObject(lookupColumn);
        Marshal.ReleaseComObject(lookupCell);
        Marshal.ReleaseComObject(lookupWorksheet);

        return resultCell.Value2;
    }
}