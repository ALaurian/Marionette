using System.Reflection;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public void ExecuteMacro(string macroName)
    {
        workbook.GetType().InvokeMember("Run", BindingFlags.Default | BindingFlags.InvokeMethod,
            null, workbook, new object[] { macroName });
    }
}