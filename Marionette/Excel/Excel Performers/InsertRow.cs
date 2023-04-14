using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public string InsertRow(object sheet, int index)
    {
        var line = ((workbook.Worksheets[sheet]).Rows[index]);
        line.Insert();
        return "Inserted new row at" + index + ".";
    }
    
    public string InsertRowModern(object sheet, int index)
    {
        var worksheet = workbook.Worksheets[sheet];
        var row = worksheet.Rows[index];
        row.Insert(XlInsertShiftDirection.xlShiftDown);

        Marshal.ReleaseComObject(row);
        Marshal.ReleaseComObject(worksheet);

        return $"Inserted new row at {index}.";
    }

}