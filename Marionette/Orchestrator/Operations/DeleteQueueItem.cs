using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public void DeleteQueueItem(QueueItem In_QueueItem, string QueueName)
    {
        // Define SQL statement to delete a row from the queue table
        string deleteRowSql = $@"
        DELETE FROM {QueueName}
        WHERE Id = '{In_QueueItem.Id}'
            AND ItemKey = '{In_QueueItem.ItemKey}';";

        // Execute the delete row SQL statement
        using (MySqlCommand command = new MySqlCommand(deleteRowSql, Connection))
        {
            command.ExecuteNonQuery();
        }
    }
}