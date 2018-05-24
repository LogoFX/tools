using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using $saferootprojectname$.lient.Data.Contracts.Dto;

namespace $safeprojectname$
{
    public class MongoWarehouseItem
    {
        public ObjectId Id { get; set; }
        [BsonElement("Kind")]
        public string Kind { get; set; }
        [BsonElement("Price")]
        public double Price { get; set; }
        [BsonElement("Quantity")]
        public int Quantity { get; set; }
        [BsonElement("ActualId")]
        public Guid ActualId { get; set; }
    }

    public class MongoUser
    {
        public ObjectId Id { get; set; }
        [BsonElement("Login")]
        public string Login { get; set; }
        [BsonElement("Password")]
        public string Password { get; set; }        
    }

    public class MongoDbSetupHelper : ISetupHelper
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _db;
        private const string DbName = "SamplesDB";

        public MongoDbSetupHelper()
        {
            _client = new MongoClient("mongodb://localhost:27017");       
            _db = _client.GetDatabase(DbName);                       
        }

        public void AddWarehouseItem(WarehouseItemDto warehouseItem)
        {
            _db.GetCollection<MongoWarehouseItem>("WarehouseItems").InsertOne(new MongoWarehouseItem
            {
                Id = new ObjectId(),
                ActualId = warehouseItem.Id,
                Kind = warehouseItem.Kind,
                Price = warehouseItem.Price,
                Quantity = warehouseItem.Quantity
            });            
        }

        public void AddUser(string login, string password)
        {
            _db.GetCollection<MongoUser>("Users").InsertOne(new MongoUser
            {
                Id = new ObjectId(),
                Login = login,
                Password = password,                
            });
        }       

        public void Initialize()
        {
            _client.DropDatabase(DbName);
        }
    }
}
