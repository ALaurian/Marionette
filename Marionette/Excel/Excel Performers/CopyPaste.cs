using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public bool CopyPaste(object sheet, string range, string destinationRange, XlPasteType pasteType)
    {
        workbook.Worksheets[sheet].Range(range).Copy();
        workbook.Worksheets[sheet].Range(destinationRange).PasteSpecial(pasteType);

        return true;
    }
    
    public bool CopyPasteModern(object sheet, string range, string destinationRange, XlPasteType pasteType)
    {
        try
        {
            var sourceRange = workbook.Worksheets[sheet].Range(range);
            var destRange = workbook.Worksheets[sheet].Range(destinationRange);
        
            sourceRange.Copy(destRange, pasteType);

            return true;
        }
        catch
        {
            return false;
        }
    }

}