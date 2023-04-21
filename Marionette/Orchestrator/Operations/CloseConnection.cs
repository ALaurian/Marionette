using System.Data;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public void CloseConnection()
    {
        try
        {
            if (Connection != null && Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Error: {0}", ex.ToString());
            throw new Exception("Unable to close the database connection.", ex);
        }
    }
}