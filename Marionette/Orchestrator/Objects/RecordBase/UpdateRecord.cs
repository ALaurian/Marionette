using MySqlConnector;

namespace Marionette.Orchestrator;

public static partial class RecordBase
{
    public static void UpdateRecord(string tableName, Dictionary<string, string> values,
        Dictionary<string, string> filters, Orchestrator _orchestrator)
    {
        var updateColumns = string.Join(", ", values.Select(v => $"{v.Key}=@{v.Key}"));
        var filterConditions = string.Join(" AND ",
            filters.Select(filter => $"{filter.Key.Replace("_prev", "")} = @{filter.Key}"));

        var updateRowSql = $@"UPDATE {tableName} SET {updateColumns} WHERE {filterConditions}";

        using (var command = new MySqlCommand(updateRowSql, _orchestrator.Connection))
        {
            foreach (var value in values)
            {
                command.Parameters.AddWithValue($"@{value.Key}", value.Value);
            }

            foreach (var filter in filters)
            {
                command.Parameters.AddWithValue($"@{filter.Key}", filter.Value);
            }

            var connectionAvailable = false;

            while (!connectionAvailable)
            {
                try
                {
                    command.ExecuteNonQuery();
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