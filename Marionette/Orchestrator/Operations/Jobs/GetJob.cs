using Marionette.Orchestrator.Enums;
using Marionette.Orchestrator.Operations.Helpers;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public Job GetJob(string process, string machine, string hostname, string hostIdentity, JobType jobType,
        RuntimeType runtimeType)
    {
        var sql = $@"
        SELECT * FROM Jobs
        WHERE Process = '{process}'
            AND Machine = '{machine}'
            AND Hostname = '{hostname}'
            AND HostIdentity = '{hostIdentity}'
            AND JobType = '{jobType}'
            AND RuntimeType = '{runtimeType}'
            LIMIT 1;"; // Added LIMIT clause here

        using (var cmd = new MySqlCommand(sql, Connection))
        {
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
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

                    return job;
                }
            }
        }

        return null;
    }
}