using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public void InvokeVBA(object sheet, string code)
    {
        // Get the VBE object
        var vbe = workbook.VBProject.VBComponents;

        // Get the active sheet's code module
        var codeModule = vbe.Item(sheet.ToString()).CodeModule;

        // Add the code to the module
        var lineNum = codeModule.CountOfLines + 1;
        codeModule.InsertLines(lineNum, code);

        // Run the code
        workbook.Application.Run("'" + sheet + "'!Module1.MacroName");

        // Remove the code from the module
        codeModule.DeleteLines(lineNum, codeModule.CountOfLines - lineNum + 1);
    }
}