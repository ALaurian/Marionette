using System.Runtime.InteropServices;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public object GetRowValue(object sheet, int index)
    {
        return (((workbook.Worksheets[sheet]).Rows[index]).Value2 as object[,]).Cast<object>().ToList();
    }
    
    public object GetRowValueModern(object sheet, int index)
    {
        var row = workbook.Worksheets[sheet].Rows[index];
        var values = row.Value2 as object[,];

        var result = new List<object>();

        for (var i = 1; i <= values.GetLength(1); i++)
        {
            result.Add(values[1, i]);
        }

        Marshal.ReleaseComObject(row);

        return result;
    }

}