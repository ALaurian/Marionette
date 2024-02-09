using MySqlConnector;

namespace Marionette.Orchestrator;

public static partial class RecordBase
{
    public static void DeleteRecord(string tableName, Dictionary<string, string> filters, Orchestrator _orchestrator)
    {
        var filterConditions = string.Join(" AND ", filters.Select(filter => $"{filter.Key} = @{filter.Key}"));

        var deleteRowSql = $@"DELETE FROM {tableName} WHERE {filterConditions}";

        using (var cmd = new MySqlCommand(deleteRowSql, _orchestrator.Connection))
        {
            foreach (var filter in filters)
            {
                cmd.Parameters.AddWithValue($"@{filter.Key}", filter.Value);
            }

            var connectionAvailable = false;

            while (!connectionAvailable)
            {
                try
                {
                    cmd.ExecuteNonQuery();
                    connectionAvailable = true;
                }
                catch (MySqlException ex)
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}