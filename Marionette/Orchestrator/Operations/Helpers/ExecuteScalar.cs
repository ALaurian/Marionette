using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    private void ExecuteScalar(string sqlCommand, out object result)
    {
        bool connectionAvailable = false;
        result = null;
        while (!connectionAvailable)
        {
            try
            {
                MySqlCommand command = new MySqlCommand(sqlCommand, Connection);

                result = command.ExecuteScalar();
                connectionAvailable = true;
            }
            catch (MySqlException ex)
            {
                Thread.Sleep(1000);
            }
        }

    }
}