using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public Process GetProcess(string name, string version)
    {
        var sql = $@"
        SELECT * FROM processes
        WHERE Name = '{name}'
            AND Version = '{version}'
            LIMIT 1;";

        using (var command = new MySqlCommand(sql, Connection))
        {
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@version", version);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var process = new Process(reader.GetString(0), reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3), this, true);
                    return process;
                }
            }
        }

        return null;
    }
}