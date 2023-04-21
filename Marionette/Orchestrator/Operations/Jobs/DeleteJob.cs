using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public void DeleteJob(Job In_Job)
    {
        string deleteRowSql = $@"
        DELETE FROM jobs
        WHERE Process = '{In_Job.Process}'
            AND Machine = '{In_Job.Machine}'
            AND Hostname = '{In_Job.Hostname}'
            AND HostIdentity = '{In_Job.HostIdentity}'
            AND JobType = '{In_Job.JobType}'
            AND RuntimeType = '{In_Job.RuntimeType}'
            AND State = '{In_Job.State}'
            AND Priority = '{In_Job.Priority}'
            AND Started = '{In_Job.Started}'
            AND Ended = '{In_Job.Ended}';";
        
        // Execute the delete row SQL statement
        ExecuteNonQuery(deleteRowSql);
    }
}