using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    private MySqlDataAdapter ReturnAdapter(string sqlCommand)
    {
        var connectionAvailable = false;
        MySqlDataAdapter adapter = null;

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

        return adapter;
    }
}