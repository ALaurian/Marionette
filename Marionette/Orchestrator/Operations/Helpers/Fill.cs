using System.Data;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    private DataTable Fill(MySqlDataAdapter adapter)
    {
        var connectionAvailable = false;

        var dataTable = new DataTable();
        while (!connectionAvailable)
        {
            try
            {
                adapter.Fill(dataTable);
                connectionAvailable = true;
            }
            catch (MySqlException ex)
            {
                Thread.Sleep(1000);
            }
        }

        return dataTable;
    }
}