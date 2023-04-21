using Marionette.Orchestrator.Enums;

namespace Marionette.Orchestrator;

public partial class Job
{
    public void Pause()
    {
        State = JobState.Suspended;
    }
}