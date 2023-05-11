using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public bool TableExists(string tableName)
    {
        var tableExists = false;

        var sqlCommand =
            $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{Connection.Database}' AND table_name = '{tableName}'";
        
        ExecuteScalar(sqlCommand, out var result);
        
        var tableCount = Convert.ToInt32(result);
        // Check if the table exists
        if (tableCount > 0)
        {
            tableExists = true;
        }

        return tableExists;
    }
    



}