using System.Runtime.InteropServices;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public object GetColumnValue(object sheet, int index)
    {
        return (((workbook.Worksheets[sheet]).Columns[index]).Value2 as object[,]).Cast<object>().ToList();
    }
    
    public object GetColumnValueModern(object sheet, int index)
    {
        var column = workbook.Worksheets[sheet].Columns[index];
        var values = column.Value2 as object[,];

        List<object> result = new List<object>();

        for (int i = 1; i <= values.GetLength(0); i++)
        {
            result.Add(values[i, 1]);
        }

        Marshal.ReleaseComObject(column);

        return result;
    }

}