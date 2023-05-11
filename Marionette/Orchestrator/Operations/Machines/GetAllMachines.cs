using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public List<Machine> GetAllMachines()
    {
        var machines = new List<Machine>();
        var sql = "SELECT * FROM machines;";

        using (var command = new MySqlCommand(sql, Connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var machine = new Machine();
                    machine.Name = reader.GetString(0);
                    machine.HostName = reader.GetString(1);
                    machine.HostIdentity = reader.GetString(2);
                    machines.Add(machine);
                }
            }
        }

        return machines;
    }
}