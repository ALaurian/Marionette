using Marionette.Orchestrator.Enums;

namespace Marionette.Orchestrator;

public partial class Job
{
    public void SetState(JobState In_State)
    {
        State = In_State;
    }
}