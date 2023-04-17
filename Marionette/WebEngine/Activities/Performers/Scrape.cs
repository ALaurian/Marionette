using System.Data;
using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    
        public DataTable Scrape(IPage page, string selector)
        {
            var dataTable = new DataTable();
            var rows = page.QuerySelectorAllAsync(selector + "/tbody/tr").Result;

            // extract header row data
            var headerRowCells = page.QuerySelectorAllAsync(selector + "/thead/tr/th").Result;
            if (headerRowCells.Any() == false)
            {
                headerRowCells = page.QuerySelectorAllAsync(selector + "/tbody/tr[1]/th").Result;
            }
    
            foreach (var headerCell in headerRowCells)
            {
                dataTable.Columns.Add(headerCell.InnerTextAsync().Result);
            }

            // extract table data
            foreach (var row in rows)
            {
                var dataCells = row.QuerySelectorAllAsync("td").Result.Where(x=> x.Equals(typeof(DBNull)) == false).ToList();
                var dataRow = dataTable.NewRow();

                for (var i = 0; i < dataCells.Count; i++)
                {
                    dataRow[i] = dataCells[i].InnerTextAsync().Result.Trim();
                }

                if (dataCells.Count != 0)
                {
                    dataTable.Rows.Add(dataRow);   
                }
        
            }

            return dataTable;
        }
    

}