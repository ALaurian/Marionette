using Marionette.Orchestrator.Enums;

namespace Marionette.Orchestrator;

public partial class Job
{
    public void Stop()
    {
        Ended = DateTime.Now.ToString();
        State = JobState.Stopping;
    }
}