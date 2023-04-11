using Marionette.Orchestrator.Enums;

namespace Marionette.Orchestrator;

public class QueueItem
{
    public QueueItem(string assignedTo, DateTime deferDate, DateTime dueDate, int id, Guid itemKey, string lastProcessingOn, Dictionary<string, object> output, QueueItemPriority priority, string progress, string queueName, string reference, int retryNo, string reviewStatus, Dictionary<string, object> specificContent, DateTime startTransactionTime, QueueItemStatus status)
    {
        AssignedTo = assignedTo;
        DeferDate = deferDate;
        DueDate = dueDate;
        Id = id;
        ItemKey = itemKey;
        LastProcessingOn = lastProcessingOn;
        Output = output;
        Priority = priority;
        Progress = progress;
        QueueName = queueName;
        Reference = reference;
        RetryNo = retryNo;
        ReviewStatus = reviewStatus;
        SpecificContent = specificContent;
        StartTransactionTime = startTransactionTime;
        Status = status;
    }

    public string AssignedTo { get; set; }
    public DateTime DeferDate { get; set; }
    public DateTime DueDate { get; set; }
    public int Id { get; set; }
    public Guid ItemKey { get; set; }
    public string LastProcessingOn { get; set; }
    public Dictionary<string, object> Output { get; set; }
    public QueueItemPriority Priority { get; set; }
    public string Progress { get; set; }
    public string QueueName { get; set; }
    public string Reference { get; set; }
    public int RetryNo { get; set; }
    public string ReviewStatus { get; set; }
    public Dictionary<string, object> SpecificContent { get; set; }
    public DateTime StartTransactionTime { get; set; }
    public QueueItemStatus Status { get; set; }
    
}