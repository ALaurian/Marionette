using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public List<string> GetWorkbookSheets()
    {
        List<string> sheetNames = new List<string>();
        foreach (Worksheet sheet in workbook.Worksheets)
        {
            sheetNames.Add(sheet.Name);
        }

        return sheetNames;
    }
}