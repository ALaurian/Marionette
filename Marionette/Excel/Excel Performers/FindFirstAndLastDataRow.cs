using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public (int firstRow, int lastRow) FindFirstAndLastDataRow(object sheet, dynamic column)
    {
        var worksheet = workbook.Worksheets[sheet];
        int lastRow = worksheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;
        Range range;

        if (column is int)
        {
            range = worksheet.Range[worksheet.Cells[1, column], worksheet.Cells[lastRow, column]];
        }
        else if (column is string)
        {
            range = worksheet.Columns[column];
        }
        else
        {
            throw new ArgumentException("Invalid column parameter");
        }

        var firstRow = 0;

        for (var row = 1; row <= lastRow; row++)
        {
            if (range.Cells[row, 1].Value2 != null)
            {
                firstRow = row;
                break;
            }
        }

        for (var row = lastRow; row >= 1; row--)
        {
            if (range.Cells[row, 1].Value2 != null)
            {
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(worksheet);
                return (firstRow, row);
            }
        }

        Marshal.ReleaseComObject(range);
        Marshal.ReleaseComObject(worksheet);
        return (0, 0);
    }
}