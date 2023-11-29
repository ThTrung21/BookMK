using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Models
{
    public class DataProvider<T> where T : class
    {
        public IMongoDatabase db;
        public IMongoCollection<T> collection;
        public static string connectionString = "mongodb://localhost:27017";
        
        public DataProvider(string collectionName)
        {
            var client = new MongoClient(connectionString);
            this.db = client.GetDatabase("BookMK");
            this.collection = this.db.GetCollection<T>(collectionName);
        }
        public void Insert(T record)
        {
            collection.InsertOne(record);
        }

        public List<T> ReadAll()
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Empty;
            return collection.Find(filter).ToList();
        }

        public List<T> ReadFiltered(FilterDefinition<T> filter)
        {
            return collection.Find(filter).ToList();
        }
        public List<string> ReadDistinctString(string PropertyName)
        {
            List<string> str = collection.Distinct<string>(PropertyName, new BsonDocument()).ToList();
            str.Sort((a, b) => string.Compare(a, b, StringComparison.Ordinal));
            return str;
        }

        public void Update(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            collection.UpdateMany(filter, update);
        }

        public void Delete(FilterDefinition<T> filter)
        {
            collection.DeleteMany(filter);
        }
        public void DeleteOne(FilterDefinition<T> filter)
        {
            collection.DeleteOne(filter);
        }
        public async Task InsertOneAsync(T record)
        {
            await collection.InsertOneAsync(record);
        }
        public async Task<List<T>> ReadAllAsync()
        {
            return await collection.Find(new BsonDocument()).ToListAsync();
        }
        public async Task<List<T>> ReadFilteredAsync(FilterDefinition<T> filter)
        {
            return await collection.Find(new BsonDocument()).ToListAsync();
        }
        public async Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            await collection.UpdateManyAsync(filter, update);
        }
        public async Task DeleteAsync(FilterDefinition<T> filter)
        {
            await collection.DeleteManyAsync(filter);
        }

    }
}
