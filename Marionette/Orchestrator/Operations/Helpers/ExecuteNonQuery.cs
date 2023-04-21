using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    private void ExecuteNonQuery(string sqlCommand)
    {
        bool connectionAvailable = false;

        while (!connectionAvailable)
        {
            try
            {
                using (MySqlCommand command = new MySqlCommand(sqlCommand, Connection))
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