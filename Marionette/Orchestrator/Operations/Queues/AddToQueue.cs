using System.Data;
using Marionette.Orchestrator.Enums;
using MySqlConnector;
using Newtonsoft.Json;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public void AddToQueue(QueueItem In_QueueItem, string QueueName)
    {
        var output = JsonConvert.SerializeObject(In_QueueItem.Output);
        var specificContent = JsonConvert.SerializeObject(In_QueueItem.SpecificContent);
        
        // Define SQL statement to insert a row into the queue table
        string insertRowSql = $@"
            INSERT INTO {QueueName} (
                AssignedTo,
                DeferDate,
                DueDate,
                Id,
                ItemKey,
                LastProcessingOn,
                Output,
                Priority,
                Progress,
                QueueName,
                Reference,
                RetryNo,
                ReviewStatus,
                SpecificContent,
                StartTransactionTime,
                Status
            )
            VALUES (
                '{In_QueueItem.AssignedTo}',
                '{In_QueueItem.DeferDate}',
                '{In_QueueItem.DueDate}',
                '{In_QueueItem.Id}',
                '{In_QueueItem.ItemKey}',
                '{In_QueueItem.LastProcessingOn}',
                '{output}',
                '{In_QueueItem.Priority}',
                '{In_QueueItem.Progress}',
                '{In_QueueItem.QueueName}',
                '{In_QueueItem.Reference}',
                '{In_QueueItem.RetryNo}',
                '{In_QueueItem.ReviewStatus}',
                '{specificContent}',
                '{In_QueueItem.StartTransactionTime}',
                '{In_QueueItem.Status}'
            );";

        // Execute the insert row SQL statement
        ExecuteNonQuery(insertRowSql);
    }
}