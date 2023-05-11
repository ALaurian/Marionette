using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    internal void ExecuteNonQuery(string sqlCommand)
    {
        var connectionAvailable = false;

        while (!connectionAvailable)
        {
            try
            {
                using (var command = new MySqlCommand(sqlCommand, Connection))
                {
                    command.ExecuteNonQuery();
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