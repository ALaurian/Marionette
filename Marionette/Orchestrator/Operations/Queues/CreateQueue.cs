using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public void CreateQueue(string QueueName)
    {
        // Define SQL statement to create the table
        string createTableSql = $@"
                CREATE TABLE {QueueName} (
                    AssignedTo TEXT,
                    DeferDate TEXT,
                    DueDate TEXT,
                    Id TEXT,
                    ItemKey TEXT,
                    LastProcessingOn TEXT,
                    Output TEXT,
                    Priority TEXT,
                    Progress TEXT,
                    QueueName TEXT,
                    Reference TEXT,
                    RetryNo TEXT,
                    ReviewStatus TEXT,
                    SpecificContent TEXT,
                    StartTransactionTime TEXT,
                    Status TEXT
                );";


        // Execute the create table SQL statement
        ExecuteNonQuery(createTableSql);
    }
}