using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureDevOpsFunctionApp
{
    public static class StorageQueueTrigger
    {
        public class AzureDevOpsEntity : TableEntity
        {
            public string Document { get; set; }
        }

        [FunctionName("StorageQueueTrigger")]
        [return: Table("azdotable", Connection = "AzureStorageConnection")]
        public static AzureDevOpsEntity Run(
            [QueueTrigger("azdoqueue", Connection = "AzureStorageConnection")]string myQueueItem,  
            ILogger log)
        {
            log.LogInformation($"Azure DevOps StorageQueueTrigger function executed.");
            var entity = JsonConvert.DeserializeObject<JObject>(myQueueItem);
            return new AzureDevOpsEntity { PartitionKey = entity["eventType"].ToString(), RowKey = entity["id"].ToString(), Document = myQueueItem };
        }
    }
}
