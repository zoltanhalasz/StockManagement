using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace StockManagement.Events
{
    public class TableStorageEntity : TableEntity
    {
        public TableStorageEntity(string partitionkey, string rowkey, string payload, string type)
        {

            this.Payload = payload;
            this.Type = type;
            this.RowKey = rowkey;
            this.PartitionKey = partitionkey;
        }
        public TableStorageEntity()
        {

        }
        public string Payload { get; set; }
        public string Type { get; set; }
        
    }
    public interface ITableStorageRepository
    {
        Task AddEvent(string partitionkey, string rowkey, string payload, string type);
        Task<IEnumerable<TableStorageEntity>> GetEventsById(Guid Id);
    }
    public class TableStorageRepository : ITableStorageRepository
    {
        const string StorageAccountConnectionString = "DefaultEndpointsProtocol=https;AccountName=zhstorageacct;AccountKey=fbCCfBorUj5K6+sfSHLDVz/gRcXGIddUdPz51u3waRDBd9SOjzKjk2yAq0nNZ/tMoypx9f7J2LUgK47tVV1L0g==;EndpointSuffix=core.windows.net";

        public async Task CreateNewTable(CloudTable table)
        {
            if (!await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("Table {0} already exists", table.Name);
                return;
            }
            Console.WriteLine("Table {0} created", table.Name);
        }

        public async Task AddEvent(string partitionkey, string rowkey, string payload, string type)
        {
            var storageaccount = CloudStorageAccount.Parse(StorageAccountConnectionString);
            var tableClient = storageaccount.CreateCloudTableClient();
            CloudTable cloudTable = tableClient.GetTableReference("EventStore");
            await CreateNewTable(cloudTable);
            TableStorageEntity eventEntity = new TableStorageEntity(partitionkey, rowkey,payload,type);              
            TableOperation tableOperation = TableOperation.Insert(eventEntity);
            await cloudTable.ExecuteAsync(tableOperation);
            Console.WriteLine("Record inserted");
            
        }

        public async Task <IEnumerable<TableStorageEntity>> GetEventsById(Guid id)
        {
            var storageaccount = CloudStorageAccount.Parse(StorageAccountConnectionString);
            var tableClient = storageaccount.CreateCloudTableClient();
            CloudTable cloudTable = tableClient.GetTableReference("EventStore");
            var query = new TableQuery<TableStorageEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id.ToString()));            
            return  await cloudTable.ExecuteQuerySegmentedAsync(query, new TableContinuationToken());                

        }

    }
}
