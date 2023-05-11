using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public List<Process> GetAllProcesses()
    {
        var processes = new List<Process>();

        var sql = "SELECT * FROM processes";
        using (var command = new MySqlCommand(sql, Connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var process = new Process(reader.GetString(0), reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3), this, true);
                    processes.Add(process);
                }
            }
        }

        return processes;
    }
}