using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using Marionette.Orchestrator.Enums;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public void ClearQueue(string QueueName)
    {
        // Define SQL statement to delete all rows from the table
        string deleteRowsSql = $@"DELETE FROM {QueueName};";

        // Execute the delete rows SQL statement
        using (MySqlCommand command = new MySqlCommand(deleteRowsSql, Connection))
        {
            command.ExecuteNonQuery();
        }
    }
}