using Marionette.Orchestrator.Enums;

namespace Marionette.Orchestrator;

public partial class Job
{
    public void SetType(JobType In_JobType)
    {
        JobType = In_JobType;
    }
}