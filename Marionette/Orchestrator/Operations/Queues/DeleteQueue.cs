using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public void DeleteQueue(string QueueName)
    {
        // Define SQL statement to drop the table
        var dropTableSql = $"DROP TABLE IF EXISTS {QueueName};";

        // Execute the drop table SQL statement
        ExecuteNonQuery(dropTableSql);
    }
}