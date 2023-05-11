using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    internal void ExecuteScalar(string sqlCommand, out object result)
    {
        var connectionAvailable = false;
        result = null;
        while (!connectionAvailable)
        {
            try
            {
                
                using (var command = new MySqlCommand(sqlCommand, Connection))
                {
                    result = command.ExecuteScalar();
                }
                
                connectionAvailable = true;
            }
            catch (MySqlException ex)
            {
                Thread.Sleep(1000);
            }
        }

    }
}