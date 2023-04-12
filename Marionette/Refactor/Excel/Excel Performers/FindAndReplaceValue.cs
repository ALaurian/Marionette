using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public int FindAndReplaceValue(object sheet, string searchValue, string replaceValue, string range = null)
    {
        var worksheet = workbook.Worksheets[sheet];
        Range searchRange;

        if (range != null)
        {
            searchRange = worksheet.Range[range];
        }
        else
        {
            int lastRow = worksheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;
            int lastColumn = worksheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Column;
            searchRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[lastRow, lastColumn]];
        }

        var searchResult = searchRange.Find(searchValue, LookIn: XlFindLookIn.xlValues);
        int replaceCount = 0;

        while (searchResult != null)
        {
            searchResult.Value2 = replaceValue;
            replaceCount++;

            searchResult = searchRange.FindNext(searchResult);
        }

        Marshal.ReleaseComObject(searchRange);
        Marshal.ReleaseComObject(worksheet);
        return replaceCount;
    }
}