using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public Machine GetMachine(string name, string hostName, string hostIdentity)
    {
        var sql = $@"
        SELECT * FROM machines
        WHERE Name = '{name}'
            AND HostName = '{hostName}'
            AND HostIdentity = '{hostIdentity}'
        LIMIT 1;";

        using (var command = new MySqlCommand(sql, Connection))
        {
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@hostName", hostName);
            command.Parameters.AddWithValue("@hostIdentity", hostIdentity);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var machine = new Machine();
                    machine.Name = reader.GetString(0);
                    machine.HostName = reader.GetString(1);
                    machine.HostIdentity = reader.GetString(2);
                    return machine;
                }
            }
        }

        return null;
    }
}