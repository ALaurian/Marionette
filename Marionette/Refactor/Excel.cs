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

public class Excel
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

    public object GetCellValue(object sheet, int row, int column)
    {
        return ((workbook.Worksheets[sheet]).Cells[row, column] as Range).Value2;
    }

    public object GetRangeValue(object sheet, string range)
    {
        return (workbook.Worksheets[sheet]).Range[range].Value2;
    }

    public object GetRowValue(object sheet, int index)
    {
        return (((workbook.Worksheets[sheet]).Rows[index]).Value2 as object[,]).Cast<object>().ToList();
    }

    public object GetColumnValue(object sheet, int index)
    {
        return (((workbook.Worksheets[sheet]).Columns[index]).Value2 as object[,]).Cast<object>().ToList();
    }

    public string SetCellValue(object sheet, int row, int column, string value)
    {
        ((workbook.Worksheets[sheet]).Range[row, column]).Value2 = value;
        return "Edited cell on row " + row + " and column " + column + " to " + value + ".";
    }

    public string SetRangeValue(object sheet, string range, string value)
    {
        (workbook.Worksheets[sheet]).Range[range].Value2 = value;
        return "Edited range " + range + " to " + value + ".";
    }

    public string DeleteCell(object sheet, int row, int column, XlDeleteShiftDirection direction)
    {
        ((workbook.Worksheets[sheet]).Cells[row, column]).Delete(direction);
        return "Deleted cell on row " + row + " and column " + column + ".";
    }

    public string DeleteRange(object sheet, string range, XlDeleteShiftDirection direction)
    {
        (workbook.Worksheets[sheet]).Range[range].Delete(direction);
        return "Deleted range " + range + ".";
    }

    public string DeleteRow(object sheet, int row, XlDeleteShiftDirection direction)
    {
        ((workbook.Worksheets[sheet]).Rows[row]).EntireRow.Delete(direction);
        return "Deleted row " + row + ".";
    }

    public string DeleteColumn(object sheet, int column, XlDeleteShiftDirection direction)
    {
        ((workbook.Worksheets[sheet]).Columns[column]).EntireColumn.Delete(direction);
        return "Deleted column " + column + ".";
    }

    public string InsertRow(object sheet, int index)
    {
        var line = ((workbook.Worksheets[sheet]).Rows[index]);
        line.Insert();
        return "Inserted new row at" + index + ".";
    }

    public string InsertColumn(object sheet, int index)
    {
        var line = ((workbook.Worksheets[sheet]).Columns[index]);
        line.Insert();
        return "Inserted new column at" + index + ".";
    }

    public void InsertDataTable(object sheet, int row, DataTable dataTable, bool deleteEntireSheet = true,
        bool dataTableHeader = true)
    {
        var dt_rows = dataTable.Rows.Cast<DataRow>();

        foreach (var dtrow in dt_rows)
        {
            dtrow.ItemArray = dtrow.ItemArray.Select(x =>
            {
                if (x.GetType() == typeof(DBNull))
                {
                    x = "";
                }

                return x;
            }).ToArray();
        }


        (workbook.Worksheets[sheet]).Activate();
        //Delete all cells
        if (deleteEntireSheet)
        {
            (workbook.Worksheets[sheet]).Cells.Clear();
        }

        var columns = dataTable.Columns.Cast<DataColumn>();
        if (dataTableHeader)
        {
            var columnNames = columns.Select(column => column.ColumnName).ToArray();
            var joinedColumnNames = Strings.Join(columnNames, "	");

            Clipboard.SetText(joinedColumnNames);
            workbook.Worksheets[sheet].Cells[row, 1].Select();
            workbook.Worksheets[sheet].Paste();
            row++;
        }

        var rows = dataTable.Rows.Cast<DataRow>();
        var itemArrayJoiner = rows.Select(r => r.ItemArray).Aggregate("",
            (current, itemArray_original) => current +
                                             (Strings.Join(
                                                 itemArray_original.Cast<string>()
                                                     .Select(item =>
                                                         item.ReplaceLineEndings("")
                                                             .Replace("\t", ""))
                                                     .ToArray(), "	") + "\n"));
        Clipboard.SetText(itemArrayJoiner);
        workbook.Worksheets[sheet].Cells[row, 1].Select();
        workbook.Worksheets[sheet].Paste();
    }

    public int GetLastRow(object sheet, int row, int column)
    {
        workbook.Worksheets[sheet].Cells[row, column].Select();
        excelApp.Selection.End(XlDirection.xlDown).Select();

        if (excelApp.Selection.Value2 is not null)
            return excelApp.Selection.Row;

        return row;
    }

    public bool CopyPaste(object sheet, string range, string destinationRange, XlPasteType pasteType)
    {
        workbook.Worksheets[sheet].Range(range).Copy();
        workbook.Worksheets[sheet].Range(destinationRange).PasteSpecial(pasteType);

        return true;
    }

    public bool AutoFill(object sheet, string range, int lastRow)
    {
        //split range by ":"
        var rangeArray = range.Split(':')[1];
        //replace all numbers in range
        var rangeArrayWithoutNumbers = Regex.Replace(rangeArray, @"\d", "");


        workbook.Worksheets[sheet].Range(range)
            .AutoFill(
                workbook.Worksheets[sheet].Range(range.Split(':')[0] + ":" + rangeArrayWithoutNumbers + lastRow),
                XlAutoFillType.xlFillCopy);
        return true;
    }

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

    public DataTable ToDataTableModern(object sheet, int headerAt = 1, bool headerless = false)
    {
        var ws = workbook.Worksheets[sheet];
        ws.Activate();

        var lastColumnWithValue = ws.Cells[headerAt, ws.Columns.Count].End(XlDirection.xlToLeft).Column;
        var lastRowWithValue = ws.Cells[ws.Rows.Count, 1].End(XlDirection.xlUp).Row;

        var newDataTable = new DataTable();

        if (!headerless)
        {
            var headerRange =
                (object[,])ws.Range[ws.Cells[headerAt, 1], ws.Cells[headerAt, lastColumnWithValue]].Value2;

            for (int i = 1; i <= headerRange.Length; i++)
            {
                string columnName;
                try
                {
                    columnName = headerRange[1, i].ToString();
                }
                catch (Exception)
                {
                    columnName = $"Blank{i}";
                }

                newDataTable.Columns.Add(columnName, typeof(string));
            }
        }
        else
        {
            for (int i = 1; i <= lastColumnWithValue; i++)
            {
                newDataTable.Columns.Add(GetExcelColumnName(i), typeof(string));
            }
        }

        var dataRange = (object[,])ws.Range[ws.Cells[headerAt + 1, 1], ws.Cells[lastRowWithValue, lastColumnWithValue]]
            .Value2;

        for (int i = 1; i <= dataRange.GetLength(0); i++)
        {
            var newRow = newDataTable.NewRow();

            for (int j = 1; j <= dataRange.GetLength(1); j++)
            {
                newRow[j - 1] = dataRange[i, j]?.ToString() ?? "";
            }

            newDataTable.Rows.Add(newRow);
        }

        return newDataTable;
    }

    public DataTable ToDataTable(object sheet, int headerAt = 1, bool headerless = false)
    {
        workbook.Worksheets[sheet].Activate();
        workbook.Worksheets[sheet].Cells[headerAt, 1].Select();

        while (excelApp.Selection.Value2 is not null)
        {
            excelApp.Selection.End(XlDirection.xlToRight).Select();
        }

        excelApp.Selection.End(XlDirection.xlToLeft).Select();
        var lastColumnWithValue = excelApp.Selection.Column;

        var lastRowWithValue = 0;
        var x = 1;
        while (x <= lastColumnWithValue)
        {
            if (lastRowWithValue < GetLastRow(sheet, headerAt, x))
                lastRowWithValue = GetLastRow(sheet, headerAt, x);

            x++;
        }


        var newDataTable = new DataTable();

        switch (headerless)
        {
            case false:
            {
                var headerRange = (workbook.Worksheets[sheet].Range[workbook.Worksheets[sheet].Cells[headerAt, 1],
                                          workbook.Worksheets[sheet].Cells[headerAt, lastColumnWithValue]]
                                      .Value2 as object[,]) ??
                                  workbook.Worksheets[sheet].Range["A1"].Value2;

                if (headerRange is string)
                {
                    var newColumn = new DataColumn(headerRange, typeof(string));
                    newDataTable.Columns.Add(newColumn);
                }
                else
                {
                    var placeholderIndex = 0;
                    foreach (string item in headerRange)
                    {
                        try
                        {
                            var newColumn = new DataColumn(item, typeof(string));
                            newDataTable.Columns.Add(newColumn);
                        }
                        catch (Exception e)
                        {
                            var newColumn = new DataColumn("Blank" + placeholderIndex, typeof(string));
                            newDataTable.Columns.Add(newColumn);
                        }
                    }
                }

                break;
            }
            case true:

                var index = 1;
                while (index <= lastColumnWithValue)
                {
                    newDataTable.Columns.Add(GetExcelColumnName(index), typeof(string));
                    index++;
                }

                break;
        }


        workbook.Worksheets[sheet].Range[workbook.Worksheets[sheet].Cells[headerAt, 1],
            workbook.Worksheets[sheet].Cells[headerAt, lastColumnWithValue]].Select();
        excelApp.Selection.Resize[lastRowWithValue, excelApp.Selection.Columns.Count].Select();

        //workbook.Worksheets[sheet].Range[excelApp.Selection, lastRowWithValue].Select();
        excelApp.Selection.Offset(1, 0).Select();
        excelApp.Selection.Resize[excelApp.Selection.Rows.Count - 1, excelApp.Selection.Columns.Count].Select();
        var selectionValue = ((object[,])excelApp.Selection.Value2).GetEnumerator();

        selectionValue.MoveNext();
        try
        {
            var rowNo = headerless ? 1 : 2;

            while (rowNo <= lastRowWithValue)
            {
                var newRow = newDataTable.NewRow();
                for (var i = 0; i < lastColumnWithValue; i++)
                {
                    if (selectionValue.Current != null)
                        newRow[i] = selectionValue.Current.ToString();
                    else
                    {
                        newRow[i] = "";
                    }

                    selectionValue.MoveNext();
                }

                newRow.ItemArray = newRow.ItemArray.Select(x =>
                {
                    if (x.GetType() == typeof(DBNull))
                    {
                        x = "";
                    }

                    return x;
                }).ToArray();

                newDataTable.Rows.Add(newRow);
                rowNo++;
            }
        }
        catch
        {
        }

        return newDataTable;
    }


    public string Close()
    {
        Retry:
        try
        {
            if (excelApp == null && workbook == null)
            {
                goto FinishLine;
            }

            workbook.Close(true);
            excelApp.Quit();

            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);
        }
        catch
        {
            goto Retry;
        }

        FinishLine:
        return "Closed Excel.";
    }

    public string SaveAs(string filePath, XlFileFormat format)
    {
        workbook.SaveAs(filePath, format);


        return "Saved Excel as " + filePath + " with format" + format + ".";
    }

    public string Save()
    {
        try
        {
            workbook.Save();
            return workbook.Path;
        }
        catch (Exception e)
        {
            try
            {
                var automation = new UIA3Automation();
                var desktop = automation.GetDesktop();
                var window = Policy.HandleResult<Window>(result => result == null)
                    .WaitAndRetry(15, retryAttempt => TimeSpan.FromSeconds(1))
                    .Execute(() => desktop.FindFirstByXPath("*[contains(@Name,'" + workbook.Name + "')]").AsWindow());

                var element = Policy.HandleResult<AutomationElement>(result => result == null)
                    .WaitAndRetry(15, interval => TimeSpan.FromSeconds(1))
                    .Execute(() => window.FindFirstByXPath("//*[@Name='Save']"));
                element.AsButton().Invoke();

                return workbook.Path;
            }
            catch (Exception exception)
            {
                throw new Exception("File failed to save.");
            }
        }
    }
}