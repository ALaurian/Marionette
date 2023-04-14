using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public void FormatCells(object sheet, string range, string fontName = "Calibri", int fontSize = 11,
        bool isBold = false,
        bool isItalic = false, bool isUnderline = false, bool isDoubleUnderline = false,
        XlHAlign horizontalAlignment = XlHAlign.xlHAlignLeft,
        XlVAlign verticalAlignment = XlVAlign.xlVAlignTop, bool wrapText = false,
        bool hasBorders = true, Color? borderColor = null,
        XlLineStyle borderLineStyle = XlLineStyle.xlLineStyleNone, XlBorderWeight borderWidth = XlBorderWeight.xlThin,
        Color? cellColor = null)
    {
        var worksheet = workbook.Worksheets[sheet];
        var cellRange = worksheet.Range[range];

        // Set the font properties
        cellRange.Font.Name = fontName;
        cellRange.Font.Size = fontSize;
        cellRange.Font.Bold = isBold;
        cellRange.Font.Italic = isItalic;
        cellRange.Font.Underline = isUnderline;
        cellRange.Font.DoubleUnderline = isDoubleUnderline;

        // Set the horizontal and vertical alignment properties
        cellRange.HorizontalAlignment = horizontalAlignment;
        cellRange.VerticalAlignment = verticalAlignment;
        cellRange.WrapText = wrapText;

        // Set the border properties
        if (hasBorders)
        {
            cellRange.BorderAround2(borderWeight: borderWidth, ColorIndex: XlColorIndex.xlColorIndexAutomatic,
                LineStyle: borderLineStyle);
        }

        if (borderColor.HasValue)
        {
            var colorIndex = ColorTranslator.ToOle(borderColor.Value);
            cellRange.Borders[XlBordersIndex.xlInsideHorizontal].Color = colorIndex;
            cellRange.Borders[XlBordersIndex.xlInsideVertical].Color = colorIndex;
            cellRange.Borders[XlBordersIndex.xlEdgeBottom].Color = colorIndex;
            cellRange.Borders[XlBordersIndex.xlEdgeLeft].Color = colorIndex;
            cellRange.Borders[XlBordersIndex.xlEdgeRight].Color = colorIndex;
            cellRange.Borders[XlBordersIndex.xlEdgeTop].Color = colorIndex;
        }

        // Set the cell background color
        if (cellColor.HasValue)
        {
            cellRange.Interior.Color = ColorTranslator.ToOle(cellColor.Value);
        }

        Marshal.ReleaseComObject(cellRange);
        Marshal.ReleaseComObject(worksheet);
    }
}