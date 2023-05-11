using MySqlConnector;

namespace Marionette.Orchestrator;

public static partial class RecordBase
{
    public static bool HasDuplicateRecord(string tableName, Dictionary<string, object> filters,
        Orchestrator _orchestrator)
    {
        var filterConditions = string.Join(" AND ", filters.Select(filter => $"{filter.Key} = @{filter.Key}"));

        var selectRowSql = $@"SELECT COUNT(*) FROM {tableName} WHERE {filterConditions}";

        using (var cmd = new MySqlCommand(selectRowSql, _orchestrator.Connection))
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
                    var result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result > 1;
                }
                catch (MySqlException ex)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        return false;
    }
}