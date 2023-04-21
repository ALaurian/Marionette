using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    private void Read(MySqlDataReader reader, out bool hasRows)
    {
        bool connectionAvailable = false;
        hasRows = false;

        while (!connectionAvailable)
        {
            try
            {
                hasRows = reader.Read();
                connectionAvailable = true;
            }
            catch (MySqlException ex)
            {
                Thread.Sleep(1000);
            }
        }
    }
}