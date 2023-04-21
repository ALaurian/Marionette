using System.Data;
using Marionette.Orchestrator.Enums;
using Newtonsoft.Json;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public List<QueueItem> ConvertDataTableToQueueItemList(DataTable dataTable)
    {
        List<QueueItem> queueItems = new List<QueueItem>();

        foreach (DataRow row in dataTable.Rows)
        {
            QueueItem queueItem = new QueueItem(
                row["AssignedTo"].ToString(),
                row["DeferDate"].ToString(),
                row["DueDate"].ToString(),
                Convert.ToInt32(row["Id"]),
                Guid.Parse(row["ItemKey"].ToString()),
                row["LastProcessingOn"].ToString(),
                JsonConvert.DeserializeObject<Dictionary<string, object>>(row["Output"].ToString()),
                (QueueItemPriority)Enum.Parse(typeof(QueueItemPriority), row["Priority"].ToString()),
                row["Progress"].ToString(),
                row["QueueName"].ToString(),
                row["Reference"].ToString(),
                Convert.ToInt32(row["RetryNo"]),
                row["ReviewStatus"].ToString(),
                JsonConvert.DeserializeObject<Dictionary<string, object>>(row["SpecificContent"].ToString()),
                row["StartTransactionTime"].ToString(),
                (QueueItemStatus)Enum.Parse(typeof(QueueItemStatus), row["Status"].ToString()), this);

            queueItems.Add(queueItem);
        }

        return queueItems;
    }
}