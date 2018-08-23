using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDBApi.DataLayer.Abstracts;
using MongoDBApi.Models;

namespace MongoDBApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: api/Products
        [HttpGet]
        public Task<IEnumerable<Products>> Get()
        {
            return _productService.GetAllProducts();
        }

        // GET: api/Products/getByName
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var product = await _productService.GetProduct(name);
                if (product == null)
                {
                    return Json("No product found!");
                }
                return Json(product);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());

            }

        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> Post(Products model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return BadRequest("Please enter product name");
                else if (string.IsNullOrWhiteSpace(model.Category))
                    return BadRequest("Please enter category");
                else if (model.Price <= 0)
                    return BadRequest("Please enter price");

                model.CreatedOn = DateTime.UtcNow;
                await _productService.AddProduct(model);
                return Ok("Your product has been added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // PUT: api/Products
        [HttpPut("{price}")]
        [Route("api/product/updatePrice")]
        public async Task<IActionResult> UpdatePrice(Products model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                return BadRequest("Product name missing");
            model.UpdatedOn = DateTime.UtcNow;
            var result = await _productService.UpdatePrice(model);
            if (result)
            {
                return Ok("Your product's price has been updated successfully");
            }
            return BadRequest("No product found to update");

        }

        // DELETE: api/Products/name
        [HttpDelete("{name}")]
        [Route("api/Products/name")]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest("Product name missing");
                var result = await _productService.RemoveProduct(name);
                if(result == false)
                {
                    return BadRequest("No product found to delete");
                }
                return Ok("Your product has been deleted successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // DELETE: api/Products
        [HttpDelete]
        public async Task<IActionResult> DeleteAllAsync()
        {
            try
            {
                var result = await _productService.RemoveAllProducts();
                if (result == false)
                {
                    return BadRequest("No product found to delete");
                }
                return Ok("Your all products has been deleted");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
