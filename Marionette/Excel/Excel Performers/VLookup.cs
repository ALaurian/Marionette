using System.Reflection;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public object VLookup(object sheet, object lookupValue, string lookupRange, int returnColumnIndex,
        bool exactMatch = true)
    {
        var range = (Range)workbook.Worksheets[sheet].Range[lookupRange];
        if (exactMatch)
        {
            range = range.Find(lookupValue, Type.Missing, XlFindLookIn.xlValues,
                XlLookAt.xlWhole, XlSearchOrder.xlByRows, XlSearchDirection.xlNext, false,
                Type.Missing, Type.Missing);
        }
        else
        {
            range = range.Find(lookupValue, Type.Missing, XlFindLookIn.xlValues,
                XlLookAt.xlPart, XlSearchOrder.xlByRows, XlSearchDirection.xlNext, false,
                Type.Missing, Type.Missing);
        }

        if (range != null)
        {
            return ((Range)workbook.Worksheets[sheet].Cells[range.Row, returnColumnIndex]).Value2;
        }
        else
        {
            return null;
        }
    }
}