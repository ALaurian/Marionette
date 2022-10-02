using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using static Flanium.WinEvents;
using Application = Microsoft.Office.Interop.Excel.Application;
using Clipboard = System.Windows.Forms.Clipboard;
using DataColumn = System.Data.DataColumn;
using DataTable = System.Data.DataTable;
using Range = System.Range;

#pragma warning disable CS0168

namespace Flanium;

[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
public class Blocks
{
    public class Engine : Dictionary<string,object>
    {
        private Func<object, object>[] _actions = Array.Empty<Func<object, object>>();
        private Dictionary<string, bool> _continueOnError = new();

        private int Index = 0;
        private int OldIndex = 0;
        private int DispatcherIndex { get; set; }
        private object[] _dispatcher { get; set; }
        private bool IsRunning { get; set; }
        private bool Jumping { get; set; }
        
        public void SetDispatcherRange(int startIndex, int endIndex)
        {
            if (_dispatcher is not null)
            {
                Range range = new(startIndex, endIndex);
                //use Range to cut the dispatcher
                _dispatcher = _dispatcher[range];  
            }
            if(_dispatcher is null)
                throw new Exception("Dispatcher is null. Please set the dispatcher first.");
        }
        
        public Engine SetDispatcher(object[] oneDimensionalArray)
        {
            _dispatcher = oneDimensionalArray;
            DispatcherIndex = 0;
            return this;
        }

        public Engine Next(string actionName)
        {
            try
            {
                DispatcherIndex++;
                if(DispatcherIndex >= _dispatcher.Length)
                    return this;
                
                JumpTo(actionName);
                return this;
            }
            catch (Exception e)
            {
                throw new Exception("No dispatcher or action name is set.", e);
            }
        }

        public object GetCurrent()
        {
            return _dispatcher[DispatcherIndex];
        }
        
        
        public Engine()
        {
        }

        public Engine(object oneDimensionalArray)
        {
            _dispatcher = oneDimensionalArray as object[];
        }
        
        public bool SetOutput(string actionName, object value)
        {
            this[actionName] = value;
            return true;
        }

        public Engine ClearAll()
        {
            _actions = Array.Empty<Func<object, object>>();
            _continueOnError = new Dictionary<string, bool>();
            return this;
        }

        public Engine AddAction(Func<object, object> action)
        {
            if (IsRunning)
            {
                throw new Exception("Cannot add action while state machine is running.");
            }

            var actionsList = _actions.ToList();
            actionsList.Add(action);
            _actions = actionsList.ToArray();

            return this;
        }

        public Engine AddActions(Func<object, object>[] actions)
        {
            if (IsRunning)
            {
                throw new Exception("Cannot add actions while state machine is running.");
            }
            
            var actionsList = _actions.ToList();
            actionsList.AddRange(actions);
            _actions = actionsList.ToArray();

            return this;
        }

        public Engine RemoveAction(string actionName)
        {
            if (IsRunning)
            {
                throw new Exception("Cannot remove action while state machine is running.");
            }

            var actionsList = _actions.ToList();
            actionsList.RemoveAll(x => x.Method.GetParameters()[0].Name == actionName);
            _actions = actionsList.ToArray();

            return this;
        }

        public Engine RemoveActions(string[] actionNames)
        {
            if (IsRunning)
            {
                throw new Exception("Cannot remove actions while state machine is running.");
            }

            var actionsList = _actions.ToList();
            actionsList.RemoveAll(x => actionNames.Contains(x.Method.GetParameters()[0].Name));
            _actions = actionsList.ToArray();

            return this;
        }
        

        public Engine AddSkipError(string actionName, bool continueOnError)
        {
            if (IsRunning)
            {
                throw new Exception("Cannot add continue on error while state machine is running.");
            }

            try
            {
                _continueOnError.Add(actionName, continueOnError);
            }
            catch (Exception e)
            {
            }

            return this;
        }

        public Engine AddSkipErrors(string[] actionNames, bool[] continueOnErrors)
        {
            if (IsRunning)
            {
                throw new Exception("Cannot add continue on errors while state machine is running.");
            }

            for (var i = 0; i < actionNames.Length; i++)
            {
                try
                {
                    _continueOnError.Add(actionNames[i], continueOnErrors[i]);
                }
                catch (Exception e)
                {
                }
            }

            return this;
        }

        public Engine RemoveSkipError(string actionName)
        {
            if (IsRunning)
            {
                throw new Exception("Cannot remove continue on error while state machine is running.");
            }

            _continueOnError.Remove(actionName);
            return this;
        }

        public Engine RemoveSkipErrors(string[] actionNames)
        {
            if (IsRunning)
            {
                throw new Exception("Cannot remove continue on errors while state machine is running.");
            }

            foreach (var actionName in actionNames)
            {
                _continueOnError.Remove(actionName);
            }

            return this;
        }

        public Engine Stop()
        {
            IsRunning = false;
            OldIndex = Index;
            return this;
        }

        public Engine Resume()
        {
            Index = OldIndex;
            Execute();
            return this;
        }

        public bool JumpTo(string actionName)
        {
            Index = Array.FindIndex(_actions, x => x.Method.GetParameters()[0].Name == actionName);
            OldIndex = Index;
            Jumping = true;
            return true;
        }

        public Engine Reset()
        {
            Index = 0;
            return this;
        }

        public Engine Execute()
        {
            if (_actions.Length == 0)
                throw new Exception("No actions to execute");
            if (IsRunning)
                throw new Exception("State machine is already running");

            IsRunning = true;

            this.Clear();
            foreach (var action in _actions)
            {
                this.Add(action.Method.GetParameters()[0].Name, null);

                var errorValue = _continueOnError.ContainsKey(action.Method.GetParameters()[0].Name);
                if (!errorValue)
                {
                    _continueOnError.Add(action.Method.GetParameters()[0].Name, false);
                }
            }

            for (var executeIndex = Index; executeIndex < _actions.Length; executeIndex++)
            {
                var a = _actions[executeIndex];

                try
                {
                    var funcOutput = a.Invoke(a);
                    this[a.Method.GetParameters()[0].Name] = funcOutput;
                    if (Jumping)
                    {
                        Jumping = false;
                        executeIndex = OldIndex-1;
                    }

                    if (IsRunning == false)
                    {
                        return this;
                    }
                }
                catch (Exception e)
                {
                    this[a.Method.GetParameters()[0].Name] = e;

                    if (_continueOnError[a.Method.GetParameters()[0].Name] == false)
                    {
                        throw;
                    }
                }
            }

            IsRunning = false;
            return this;
        }
    }

    public class ExcelEngine
    {
        private Application excelApp { get; set; }
        private Workbook workbook { get; set; }

        public Workbook GetWorkbook()
        {
            return workbook;
        }

        public ExcelEngine(string filePath)
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
            return workbook.Worksheets[sheet].Cells[row, column].Value2;
        }

        public object GetRangeValue(object sheet, string range)
        {
            return workbook.Worksheets[sheet].Range[range].Value2;
        }

        public object GetRowValue(object sheet, int index)
        {
            return (workbook.Worksheets[sheet].Rows[index].Value2 as object[,]).Cast<object>().ToList();
        }

        public object GetColumnValue(object sheet, int index)
        {
            return (workbook.Worksheets[sheet].Columns[index].Value2 as object[,]).Cast<object>().ToList();
        }

        public string SetCellValue(object sheet, int row, int column, string value)
        {
            workbook.Worksheets[sheet].Cells[row, column].Value2 = value;
            return "Edited cell on row " + row + " and column " + column + " to " + value + ".";
        }

        public string SetRangeValue(object sheet, string range, string value)
        {
            workbook.Worksheets[sheet].Range[range].Value2 = value;
            return "Edited range " + range + " to " + value + ".";
        }

        public string DeleteCell(object sheet, int row, int column, XlDeleteShiftDirection direction)
        {
            workbook.Worksheets[sheet].Cells[row, column].Delete(direction);
            return "Deleted cell on row " + row + " and column " + column + ".";
        }

        public string DeleteRange(object sheet, string range, XlDeleteShiftDirection direction)
        {
            workbook.Worksheets[sheet].Range[range].Delete(direction);
            return "Deleted range " + range + ".";
        }

        public string DeleteRow(object sheet, int row, XlDeleteShiftDirection direction)
        {
            workbook.Worksheets[sheet].Rows[row].EntireRow.Delete(direction);
            return "Deleted row " + row + ".";
        }

        public string DeleteColumn(object sheet, int column, XlDeleteShiftDirection direction)
        {
            workbook.Worksheets[sheet].Columns[column].EntireColumn.Delete(direction);
            return "Deleted column " + column + ".";
        }

        public string InsertRow(object sheet, int index)
        {
            var line = workbook.Worksheets[sheet].Rows[index];
            line.Insert();
            return "Inserted new row at" + index + ".";
        }

        public string InsertColumn(object sheet, int index)
        {
            var line = workbook.Worksheets[sheet].Columns[index];
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

            
            workbook.Worksheets[sheet].Activate();
            //Delete all cells
            if (deleteEntireSheet)
            {
                workbook.Worksheets[sheet].Cells.Clear();
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
                        workbook.Worksheets[sheet].Cells[headerAt, lastColumnWithValue]].Value2 as object[,]) ?? workbook.Worksheets[sheet].Range["A1"].Value2;
                    
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
            var selectionValue = ((object[,]) excelApp.Selection.Value2).GetEnumerator();

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
            var window = Search.GetWindow("*[contains(@Name,'" + workbook.Name + "')]");
            var saveButton = Search.FindElement(window, "//*[@Name='Save']");
            WinEvents.Action.Click(saveButton, true);

            return "Saved Excel file.";
        }
    }

    public class DataTableEngine
    {
        private DataTable _dataTable { get; set; }

        public string Cells(int row, object column, string value)
        {
            if (column is int)
                _dataTable.Rows[row][(int) column] = value;
            else
                _dataTable.Rows[row][column.ToString()] = value;
            return "Edited cell on row " + row + " and column " + column + " to " + value + ".";
        }

        public string Cells(int row, object column)
        {
            if (column is int)
                return _dataTable.Rows[row][(int) column].ToString();
            else
                return _dataTable.Rows[row][column.ToString()].ToString();
        }

        public DataRow Row(int index)
        {
            return _dataTable.Rows[index];
        }

        public string UpdateRow(int index, DataRow row)
        {
            _dataTable.Rows[index].ItemArray = row.ItemArray;
            return "Updated row at " + index + ".";
        }

        public string DeleteRow(int index)
        {
            _dataTable.Rows[index].Delete();
            return "Deleted row at " + index + ".";
        }

        public DataTableEngine(DataTable dataTable)
        {
            _dataTable = dataTable;
        }

        public DataTableEngine Filter(Func<DataRow, bool> linQFunction)
        {
            var filteredDataTableRows = _dataTable.Rows.Cast<DataRow>()
                .Where(linQFunction);

            var newDataTable = new DataTable();
            foreach (var c in _dataTable.Columns.Cast<DataColumn>())
            {
                newDataTable.Columns.Add(c.ColumnName);
            }

            foreach (var row in filteredDataTableRows)
            {
                var newRow = newDataTable.NewRow();
                newRow.ItemArray = row.ItemArray;
                newDataTable.Rows.Add(newRow);
            }

            _dataTable = null;
            _dataTable = newDataTable;
            return this;
        }

        public DataTableEngine ForEach(Func<DataRow, DataRow> linQFunction)
        {
            var filteredDataTableRows = _dataTable.Rows.Cast<DataRow>()
                .Select(linQFunction);

            var newDataTable = new DataTable();
            foreach (var c in _dataTable.Columns.Cast<DataColumn>())
            {
                newDataTable.Columns.Add(c.ColumnName);
            }

            foreach (var row in filteredDataTableRows)
            {
                var newRow = newDataTable.NewRow();
                newRow.ItemArray = row.ItemArray;
                newDataTable.Rows.Add(newRow);
            }

            _dataTable = null;
            _dataTable = newDataTable;
            return this;
        }

        public DataTable GetDataTable()
        {
            return _dataTable;
        }
    }
}