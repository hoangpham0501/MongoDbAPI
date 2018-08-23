using MongoDB.Driver;
using MongoDBApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBApi.DataLayer.Abstracts
{
    public interface IProductService
    {
        Task<IEnumerable<Products>> GetAllProducts();
        Task<Products> GetProduct(string name);
        Task AddProduct(Products model);
        Task<bool> UpdatePrice(Products model);
        Task<bool> RemoveProduct(string name);
        Task<bool> RemoveAllProducts();
    }
}
