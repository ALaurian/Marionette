using Cronos;
using Marionette.Orchestrator.Enums;

namespace Marionette.Orchestrator;

public partial class Trigger
{
    public string Name;
    public Job Job = null;
    public Machine Machine = null;
    public List<Job> ListOfJobs;
    public string Cron;
    public List<DateTime> Occurences;
    public List<DateTime> SavedOccurences;
    private Orchestrator _orchestrator;

    public void AddJob()
    {
        ListOfJobs.Add(Job);
    }

    public void GetOccurences()
    {
        SavedOccurences = CronExpression.Parse(Cron)
            .GetOccurrences(DateTime.UtcNow,
            DateTime.UtcNow.AddMonths(1),
            fromInclusive: true,
            toInclusive: false).ToList();
    }
    
    private void SaveOccurences()
    {
        Occurences = SavedOccurences;
    }

    private void ClearOccurences()
    {
		Occurences = new List<DateTime>();
	}
    
}