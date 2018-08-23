using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDBApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBApi.DataLayer
{
    public class MongoRepository
    {
        //delcaring mongodb
        private readonly IMongoDatabase _database;
        public MongoRepository(IOptions<Settings> settings)
        {
            try
            {
                var client = new MongoClient(settings.Value.ConnectionString);
                if(client != null)
                {
                    _database = client.GetDatabase(settings.Value.Database);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Can not access to MongoDb server.", ex);
            }
        }

        public IMongoCollection<Products> products => _database.GetCollection<Products>("Products");
    }
}
