using MySqlConnector;
using Newtonsoft.Json;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public void UpdateQueueItem(QueueItem In_QueueItem, string QueueName)
    {
        var output = JsonConvert.SerializeObject(In_QueueItem.Output);
        var specificContent = JsonConvert.SerializeObject(In_QueueItem.SpecificContent);
        
        // Define SQL statement to update a row in the queue table
        string updateRowSql = $@"
        UPDATE {QueueName}
        SET
            AssignedTo = '{In_QueueItem.AssignedTo}',
            DeferDate = '{In_QueueItem.DeferDate}',
            DueDate = '{In_QueueItem.DueDate}',
            LastProcessingOn = '{In_QueueItem.LastProcessingOn}',
            Output = '{output}',
            Priority = '{In_QueueItem.Priority}',
            Progress = '{In_QueueItem.Progress}',
            Reference = '{In_QueueItem.Reference}',
            RetryNo = '{In_QueueItem.RetryNo}',
            ReviewStatus = '{In_QueueItem.ReviewStatus}',
            SpecificContent = '{specificContent}',
            StartTransactionTime = '{In_QueueItem.StartTransactionTime}',
            Status = '{In_QueueItem.Status}'
        WHERE Id = '{In_QueueItem.Id}'
            AND ItemKey = '{In_QueueItem.ItemKey}';";

        // Execute the update row SQL statement
        using (MySqlCommand command = new MySqlCommand(updateRowSql, Connection))
        {
            command.ExecuteNonQuery();
        }
    }
}