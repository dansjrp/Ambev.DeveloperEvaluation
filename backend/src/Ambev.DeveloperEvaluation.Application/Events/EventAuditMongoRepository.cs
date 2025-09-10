using MongoDB.Driver;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Events
{
    public class EventAuditMongoRepository
    {
        private readonly IMongoCollection<dynamic> _collection;

        public EventAuditMongoRepository(string connectionString, string database, string collection)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(database);
            _collection = db.GetCollection<dynamic>(collection);
        }

        public async Task StoreEventAsync(object evt)
        {
            await _collection.InsertOneAsync(evt);
        }
    }
}
