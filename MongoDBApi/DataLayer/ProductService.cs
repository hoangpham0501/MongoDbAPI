using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBApi.DataLayer.Abstracts;
using MongoDBApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBApi.DataLayer
{
    public class ProductService : IProductService
    {
        private readonly MongoRepository _repository = null;
        public ProductService(IOptions<Settings> settings)
        {
            _repository = new MongoRepository(settings);
        }

        public async Task<IEnumerable<Products>> GetAllProducts()
        {
            try
            {
                return await _repository.products.Find(x => true).ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<Products> GetProduct(string name)
        {
            var filter = Builders<Products>.Filter.Eq(x => x.Name, name);
            return await _repository.products.Find(filter).FirstOrDefaultAsync();
        }
        public async Task AddProduct(Products model)
        {
            //inserting data
            await _repository.products.InsertOneAsync(model);
        }

        public async Task<bool> UpdatePrice(Products model)
        {

            var filter = Builders<Products>.Filter.Eq("Name", model.Name);
            var product = _repository.products.Find(filter).FirstOrDefaultAsync();
            if (product.Result == null)
                return false;
            var update = Builders<Products>.Update
                                          .Set(x => x.Price, model.Price)
                                          .Set(x => x.UpdatedOn, model.UpdatedOn);

            await _repository.products.UpdateOneAsync(filter, update);
            return true;
        }

        public async Task<bool> RemoveProduct(string name)
        {
            var filter = Builders<Products>.Filter.Eq("Name", name);
            var writeConcernResult = await _repository.products.DeleteOneAsync(filter);
            return await Task.FromResult(writeConcernResult.DeletedCount == 1);
        }
        public async Task<bool> RemoveAllProducts()
        {
            var writeConcernResult = await _repository.products.DeleteManyAsync(new BsonDocument());
            return await Task.FromResult(writeConcernResult.DeletedCount != 0);
        }
    }
}
