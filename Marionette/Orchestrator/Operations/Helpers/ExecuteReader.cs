using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    private void ExecuteReader(string sqlCommand, out MySqlDataReader dataReader)
    {
        bool connectionAvailable = false;
        dataReader = null;

        while (!connectionAvailable)
        {
            try
            {
                using (MySqlCommand command = new MySqlCommand(sqlCommand, Connection))
                {
                    dataReader = command.ExecuteReader();
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