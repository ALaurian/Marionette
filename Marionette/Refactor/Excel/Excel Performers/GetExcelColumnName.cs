namespace Marionette.Excel_Scope;

public partial class Excel
{
    private string GetExcelColumnName(int columnNumber)
    {
        string columnName = "";

        while (columnNumber > 0)
        {
            int modulo = (columnNumber - 1) % 26;
            columnName = Convert.ToChar('A' + modulo) + columnName;
            columnNumber = (columnNumber - modulo) / 26;
        }

        return columnName;
    }
    
    private string GetExcelColumnNameModern(int columnNumber)
    {
        char[] columnName = new char[3];
        int index = 2;

        while (columnNumber > 0)
        {
            int modulo = (columnNumber - 1) % 26;
            columnName[index--] = (char)('A' + modulo);
            columnNumber = (columnNumber - modulo) / 26;
        }

        return new string(columnName, index + 1, 2 - index);
    }

}