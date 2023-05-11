using Marionette.Orchestrator.Operations.Helpers;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public List<Trigger> GetAllTriggers()
    {
        var triggers = new List<Trigger>();
        var sql = "SELECT * FROM triggers;";

        using (var command = new MySqlCommand(sql, Connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var trigger = new Trigger();

                    trigger.Name = reader.GetString(0);
                    trigger.Job = DatabaseObjectHandler.Deserialize<Job>(reader.GetString(1));
                    trigger.Machine = DatabaseObjectHandler.Deserialize<Machine>(reader.GetString(2));
                    trigger.ListOfJobs = DatabaseObjectHandler.Deserialize<List<Job>>(reader.GetString(3));
                    trigger.Cron = reader.GetString(4);
                    trigger.Occurences = DatabaseObjectHandler.Deserialize<List<DateTime>>(reader.GetString(5));
                    trigger.SavedOccurences = DatabaseObjectHandler.Deserialize<List<DateTime>>(reader.GetString(6));

                    triggers.Add(trigger);
                }
            }
        }

        return triggers;
    }
}