using MySqlConnector;

namespace Marionette.Orchestrator
{
	public static partial class RecordBase
	{
		public static void CreateRecord(string tableName, Dictionary<string, string> values, Orchestrator _orchestrator)
		{
			var columns = string.Join(", ", values.Keys);
			var parameterNames = string.Join(", ", values.Keys.Select(key => $"@{key}"));
			var insertRowSql = $"INSERT INTO {tableName} ({columns}) VALUES ({parameterNames})";
			
			using (var cmd = new MySqlCommand(insertRowSql, _orchestrator.Connection))
			{
				foreach (var kvp in values)
				{
					cmd.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
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
}