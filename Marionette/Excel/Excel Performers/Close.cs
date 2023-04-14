using System.Runtime.InteropServices;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public bool Close()
    {
        if (excelApp == null || workbook == null)
        {
            return true;
        }

        var retryCount = 0;

        while (retryCount < 10)
        {
            try
            {
                workbook.Close(true);
                excelApp.Quit();

                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(excelApp);

                return true;
            }
            catch
            {
                retryCount++;
                Thread.Sleep(1000);
            }
        }

        return false;
    }

    
}