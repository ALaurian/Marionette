using Marionette.Orchestrator.Enums;

namespace Marionette.Orchestrator;

public partial class Job
{
    public void SetPriority(JobPriority In_Priority)
    {
        Priority = In_Priority;
    }
}