using Marionette.Orchestrator.Enums;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public void UpdateJob(Job In_Job)
    {
        // Define SQL statement to update a row in the jobs table
        string updateRowSql = $@"
        UPDATE jobs
        SET
            Process = '{In_Job.Process}',
            Machine = '{In_Job.Machine}',
            Hostname = '{In_Job.Hostname}',
            HostIdentity = '{In_Job.HostIdentity}',
            JobType = '{In_Job.JobType}',
            RuntimeType = '{In_Job.RuntimeType}',
            State = '{In_Job.State}',
            Priority = '{In_Job.Priority}',
            Started = '{In_Job.Started}',
            Ended = '{In_Job.Ended}'
        WHERE
            Process = '{In_Job.Process}' AND
            Machine = '{In_Job.Machine}' AND
            Hostname = '{In_Job.Hostname}' AND
            HostIdentity = '{In_Job.HostIdentity}' AND
            JobType = '{In_Job.JobType}' AND
            RuntimeType = '{In_Job.RuntimeType}' AND
            State = '{In_Job.State}' AND
            Priority = '{In_Job.Priority}';";

        // Execute the update row SQL statement
        using (MySqlCommand command = new MySqlCommand(updateRowSql, Connection))
        {
            command.ExecuteNonQuery();
        }
    }
}