using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public void TextToColumns(object sheet, string range, string delimiter)
    {
        var worksheet = workbook.Worksheets[sheet];
        var targetRange = worksheet.Range[range];
        targetRange.TextToColumns(targetRange, XlTextParsingType.xlDelimited, XlTextQualifier.xlTextQualifierNone,
            XlColumnDataType.xlGeneralFormat, null, true, false, false, false, false, false,
            delimiter);

        Marshal.ReleaseComObject(targetRange);
        Marshal.ReleaseComObject(worksheet);
    }
}