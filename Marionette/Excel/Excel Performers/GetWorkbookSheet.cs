namespace Marionette.Excel_Scope;

public partial class Excel
{
    public string GetWorkbookSheet(int index)
    {
        if (index < 1 || index > workbook.Worksheets.Count)
        {
            throw new ArgumentException("Invalid worksheet index.");
        }

        return workbook.Worksheets[index].Name;
    }
}