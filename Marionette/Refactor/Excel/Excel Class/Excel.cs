using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Polly;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;
using Range = Microsoft.Office.Interop.Excel.Range;
using Window = FlaUI.Core.AutomationElements.Window;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    private Application excelApp { get; set; }
    private Workbook workbook { get; set; }

    public Workbook GetWorkbook()
    {
        return workbook;
    }

    public Excel(string filePath)
    {
        try
        {
            excelApp = new()
            {
                Visible = true,
                DisplayAlerts = false
            };
            try
            {
                workbook.Close();
            }
            catch
            {
                // ignored
            }

            workbook = excelApp.Workbooks.Open(filePath);
        }
        catch (Exception e)
        {
            throw new Exception("Error occurred while opening excel file.", e);
        }
    }
}