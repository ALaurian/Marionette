using Marionette.Orchestrator.Enums;
using Marionette.Orchestrator.Operations.Helpers;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public List<Job> GetAllJobs()
    {
        var jobs = new List<Job>();

        var sql = "SELECT * FROM Jobs";
        using (var cmd = new MySqlCommand(sql, Connection))
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var job = new Job(
                        DatabaseObjectHandler.Deserialize<Process>(reader["Process"].ToString()),
                        DatabaseObjectHandler.Deserialize<Machine>(reader["Machine"].ToString()),
                        (JobType)Enum.Parse(typeof(JobType), reader["JobType"].ToString()),
                        (RuntimeType)Enum.Parse(typeof(RuntimeType), reader["RuntimeType"].ToString()),
                        (JobState)Enum.Parse(typeof(JobState), reader["State"].ToString()),
                        (JobPriority)Enum.Parse(typeof(JobPriority), reader["Priority"].ToString()),
                        reader["Started"].ToString(),
                        reader["Ended"].ToString(),
                        this,
                        true
                    );

                    jobs.Add(job);
                }
            }
        }

        return jobs;
    }
}