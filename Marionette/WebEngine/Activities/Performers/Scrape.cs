using System.Data;
using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    
    public DataTable Scrape(IPage page, string selector)
    {
        var tableElement = FindElement(selector);

        if (tableElement == null)
        {
            throw new Exception("The element was not found.");
        }
    
        if (tableElement.GetAttributeAsync("tag").Result != "table")
        {
            throw new Exception("The queried selector is not a table.");
        }

        var dataTable = new DataTable();
        var rows = page.QuerySelectorAllAsync(selector + "/tbody/tr").Result;

        // extract header row data
        var headerRowCells = page.QuerySelectorAllAsync(selector + "/thead/tr/th").Result;
        if (headerRowCells.Any() == false)
        {
            headerRowCells = page.QuerySelectorAllAsync(selector + "/thead/tr[1]").Result;
        }
        
        foreach (var headerCell in headerRowCells)
        {
            dataTable.Columns.Add(headerCell.InnerTextAsync().Result.Trim());
        }

        // extract table data
        foreach (var row in rows)
        {
            var dataCells = row.QuerySelectorAllAsync("td").Result;
            var dataRow = dataTable.NewRow();

            for (var i = 0; i < dataCells.Count; i++)
            {
                dataRow[i] = dataCells[i].InnerTextAsync().Result.Trim();
            }

            dataTable.Rows.Add(dataRow);
        }

        return dataTable;
    }
    

}