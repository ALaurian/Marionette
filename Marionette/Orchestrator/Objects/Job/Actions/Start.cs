using Marionette.Orchestrator.Enums;

namespace Marionette.Orchestrator;

public partial class Job
{
    public void Start()
    {
        Started = DateTime.Now.ToString();
        State = JobState.Running;
    }
}