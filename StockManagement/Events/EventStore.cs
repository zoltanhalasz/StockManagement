using StockManagement.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;

namespace StockManagement.Events
{
    public interface IEventStore
    {
        void Save<T>(T aggr) where T: IAggregate;

        Task<T> GetAggregateByID<T>(Guid id) where T: IAggregate, new();
        
    }
    public class EventStore : IEventStore
    {
        ITableStorageRepository tstorage;
        public EventStore(ITableStorageRepository tstorage)
        {
            this.tstorage = tstorage;
        }
        public async void Save<T>(T aggr) where T : IAggregate
        {
            var rnd = new Random();
            foreach (var myevent in aggr.PendingEvents)
            {                
                var partitionkey = aggr.Id.ToString();
                var rowKey = partitionkey + "-" + rnd.Next();
                await tstorage.AddEvent(partitionkey, rowKey, Newtonsoft.Json.JsonConvert.SerializeObject(myevent), myevent.GetType().ToString());
            }
            
        }


        public async Task<T> GetAggregateByID<T>(Guid id) where T : IAggregate, new()
        {
            var tableStorageEntityList = await tstorage.GetEventsById(id);
            var eventList = ConvertTableEntityListToEventList(tableStorageEntityList);
            var aggr= new T() { };
            aggr.ReconstituteFromHistory(eventList);
            return aggr;
        }

        private IEnumerable<IEvent> ConvertTableEntityListToEventList(IEnumerable<TableStorageEntity> inputTable)
        {
            inputTable = inputTable.OrderBy(x => x.Timestamp).ToList();
            var eventList = new List<IEvent>();
            foreach (var tableEntity in inputTable)
            {
                var type = Type.GetType(tableEntity.Type);
                var eventDeserialized = JsonConvert.DeserializeObject(tableEntity.Payload, type);
                eventList.Add(eventDeserialized as IEvent);
            }

            return eventList;
        }
    }
}
