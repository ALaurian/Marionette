using Marionette.Orchestrator.Enums;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public void CreateJob(string Process, string Machine, string Hostname, string HostIdentity, JobType JobType,
        RuntimeType RuntimeType, JobState State, JobPriority Priority, string Started, string Ended)
    {
        string insertRowSql = $@"
            INSERT INTO jobs (
                Process,
                Machine,
                Hostname,
                Host Identity,
                Job Type,
                Runtime Type,
                State,
                Priority,
                Started,
                Ended
            )
            VALUES (
                '{Process}',
                '{Machine}',
                '{Hostname}',
                '{HostIdentity}',
                '{JobType}',
                '{RuntimeType}',
                '{State}',
                '{Priority}',
                '{Started}',
                '{Ended}'
            );";

        // Execute the insert row SQL statement
        ExecuteNonQuery(insertRowSql);
    }
}