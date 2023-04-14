using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{

    public string SaveAs(string filePath, XlFileFormat format)
    {
        workbook.SaveAs(filePath, format);

        return $"Saved Excel as {filePath} with format {format}.";
    }
}