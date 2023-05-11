using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    internal void ExecuteReader(string sqlCommand, out MySqlDataReader dataReader)
    {
        var connectionAvailable = false;
        dataReader = null;

        while (!connectionAvailable)
        {
            try
            {
                using (var command = new MySqlCommand(sqlCommand, Connection))
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