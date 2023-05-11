using System.Data;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    private void Fill(MySqlDataAdapter adapter, out DataTable dataTable)
    {
        var connectionAvailable = false;

        dataTable = new DataTable();
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
    }
}