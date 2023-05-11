using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public void SortRange(object sheet, string column = null, string range = null, bool ascending = true)
    {
        if (column != null)
        {
            var sortRange = (Range)workbook.Worksheets[sheet].Columns[column];
            sortRange.Sort(sortRange.Columns[1], ascending ? XlSortOrder.xlAscending : XlSortOrder.xlDescending,
                Type.Missing, Type.Missing, XlSortOrder.xlAscending,
                Type.Missing, XlSortOrder.xlAscending, XlYesNoGuess.xlYes,
                Type.Missing, Type.Missing, XlSortOrientation.xlSortColumns, XlSortMethod.xlPinYin,
                XlSortDataOption.xlSortNormal, XlSortDataOption.xlSortNormal, XlSortDataOption.xlSortNormal);
        }
        else if (range != null)
        {
            var sortRange = (Range)workbook.Worksheets[sheet].Range[range];
            sortRange.Sort(sortRange.Columns[1], ascending ? XlSortOrder.xlAscending : XlSortOrder.xlDescending,
                Type.Missing, Type.Missing, XlSortOrder.xlAscending,
                Type.Missing, XlSortOrder.xlAscending, XlYesNoGuess.xlYes,
                Type.Missing, Type.Missing, XlSortOrientation.xlSortColumns, XlSortMethod.xlPinYin,
                XlSortDataOption.xlSortNormal, XlSortDataOption.xlSortNormal, XlSortDataOption.xlSortNormal);
        }
        else
        {
            throw new ArgumentException("Either column or range must be specified.");
        }
    }
}