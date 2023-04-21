using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    private void ReturnAdapter(string sqlCommand, out MySqlDataAdapter adapter)
    {
        bool connectionAvailable = false;
        adapter = null;

        while (!connectionAvailable)
        {
            try
            {
                var command = new MySqlCommand(sqlCommand, Connection);
                adapter = new MySqlDataAdapter(command);
                connectionAvailable = true;
            }
            catch (MySqlException ex)
            {
                Thread.Sleep(1000);
            }
        }
    }
}