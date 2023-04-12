using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public void FormatAsTable(object sheet, string range, string tableName, bool hasHeaders)
    {
        var worksheet = workbook.Worksheets[sheet];
        var tableRange = worksheet.Range[range];

        var table = worksheet.ListObjects.Add(XlListObjectSourceType.xlSrcRange,
            tableRange,
            Type.Missing,
            XlYesNoGuess.xlYes,
            Type.Missing);

        table.Name = tableName;

        if (hasHeaders)
        {
            table.HeaderRowRange.Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
        }

        Marshal.ReleaseComObject(table);
        Marshal.ReleaseComObject(tableRange);
        Marshal.ReleaseComObject(worksheet);
    }
}