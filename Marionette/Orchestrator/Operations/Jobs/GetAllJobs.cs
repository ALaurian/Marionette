using Marionette.Orchestrator.Enums;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public List<Job> GetAllJobs()
    {
        // Define SQL statement to select all rows from the jobs table
        string selectRowsSql = $@"SELECT * FROM jobs;";

        // Execute the select rows SQL statement
        using (MySqlCommand command = new MySqlCommand(selectRowsSql, Connection))
        {
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                List<Job> jobs = new List<Job>();
                while (reader.Read())
                {
                    JobType.TryParse(reader["JobType"].ToString(), out JobType jobType);
                    RuntimeType.TryParse(reader["RuntimeType"].ToString(), out RuntimeType runtimeType);
                    JobState.TryParse(reader["State"].ToString(), out JobState jobState);
                    JobPriority.TryParse(reader["Priority"].ToString(), out JobPriority jobPriority);

                    //Create the job object using the constructor
                    Job job = new Job(reader.GetString("Process"),
                        reader.GetString("Machine"),
                        reader.GetString("Hostname"),
                        reader.GetString("HostIdentity"),
                        jobType,
                        runtimeType,
                        jobState,
                        jobPriority,
                        reader.GetString("Started"),
                        reader.GetString("Ended"));

                    jobs.Add(job);
                }

                return jobs;
            }
        }
    }
}