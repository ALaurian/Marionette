using System.Data;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    private void OpenConnection(string connectionString)
    {
        try
        {
            if (Connection == null || Connection.State != ConnectionState.Open)
            {
                Connection = new MySqlConnection(connectionString);
                Connection.Open();
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Error: {0}", ex.ToString());
            throw new Exception("Unable to establish connection to the database.", ex);
        }
    }
}