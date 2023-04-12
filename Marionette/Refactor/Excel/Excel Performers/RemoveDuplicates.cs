using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public void RemoveDuplicates(object sheet, string range, int[] columnsToCompare = null)
    {
        var worksheet = workbook.Worksheets[sheet];
        var usedRange = worksheet.UsedRange;
        var dataRange = usedRange.Range[range];

        // Build the array of columns to compare
        int[] columnIndexArray;
        if (columnsToCompare == null)
        {
            columnIndexArray = Enumerable.Range(dataRange.Column, dataRange.Columns.Count).ToArray();
        }
        else
        {
            columnIndexArray = new int[columnsToCompare.Length];
            for (var i = 0; i < columnsToCompare.Length; i++)
            {
                columnIndexArray[i] = columnsToCompare[i] + dataRange.Column - 1;
            }
        }

        // Remove duplicates
        dataRange.RemoveDuplicates(columnIndexArray, XlYesNoGuess.xlYes);
    }
}