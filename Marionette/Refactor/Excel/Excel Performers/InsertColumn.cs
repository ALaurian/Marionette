using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public string InsertColumn(object sheet, int index)
    {
        var line = ((workbook.Worksheets[sheet]).Columns[index]);
        line.Insert();
        return "Inserted new column at" + index + ".";
    }
    
    public string InsertColumnModern(object sheet, int index)
    {
        var worksheet = workbook.Worksheets[sheet];
        var column = worksheet.Columns[index];
        column.Insert(XlInsertShiftDirection.xlShiftToRight);

        Marshal.ReleaseComObject(column);
        Marshal.ReleaseComObject(worksheet);

        return $"Inserted new column at {index}.";
    }

}