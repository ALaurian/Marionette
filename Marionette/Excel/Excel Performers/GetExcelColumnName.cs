namespace Marionette.Excel_Scope;

public partial class Excel
{
    private string GetExcelColumnName(int columnNumber)
    {
        var columnName = "";

        while (columnNumber > 0)
        {
            var modulo = (columnNumber - 1) % 26;
            columnName = Convert.ToChar('A' + modulo) + columnName;
            columnNumber = (columnNumber - modulo) / 26;
        }

        return columnName;
    }
    
    private string GetExcelColumnNameModern(int columnNumber)
    {
        var columnName = new char[3];
        var index = 2;

        while (columnNumber > 0)
        {
            var modulo = (columnNumber - 1) % 26;
            columnName[index--] = (char)('A' + modulo);
            columnNumber = (columnNumber - modulo) / 26;
        }

        return new string(columnName, index + 1, 2 - index);
    }

}