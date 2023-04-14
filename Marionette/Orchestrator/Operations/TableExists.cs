using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public bool TableExists(string tableName)
    {
        bool tableExists = false;

        // Create a MySqlCommand object to execute a SQL query
        using (MySqlCommand command =
               new MySqlCommand(
                   $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{Connection.Database}' AND table_name = '{tableName}'",
                   Connection))
        {
            // Execute the SQL query and get the result
            int tableCount = Convert.ToInt32(command.ExecuteScalar());

            // Check if the table exists
            if (tableCount > 0)
            {
                tableExists = true;
            }
        }

        return tableExists;
    }
}