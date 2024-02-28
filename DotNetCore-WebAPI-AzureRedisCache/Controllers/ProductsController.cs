using DotNetCore_WebAPI_AzureRedisCache.IRepository;
using DotNetCore_WebAPI_AzureRedisCache.Models;
using DotNetCore_WebAPI_AzureRedisCache.Repository;
using DotNetCore_WebAPI_AzureRedisCache.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore_WebAPI_AzureRedisCache.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		IProduct _product;
		ICache _cache;
		AzureServiceBusService _azureservicebusservice;
		public ProductsController(IProduct product, ICache cache, AzureServiceBusService service)
		{
			_product = product;
			_cache = cache;
			_azureservicebusservice = service;
		}
		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			var cachedProducts = await _cache.GetCacheData<List<Product>>("Products");
			if (cachedProducts != null)
			{
				return Ok(cachedProducts);
			}
			var products = _product.GetProducts();
			if( products.Count > 0)
			{
				await _cache.SetCacheData("Products", products, DateTimeOffset.Now.AddMinutes(2));
			}
			
			return Ok(products) ;
		}
		[HttpPost]
		public async Task<ActionResult> AddProducts(Product product)
		{
			var response =  _product.AddProduct(product);
			await _azureservicebusservice.SendProductToQueueAsync(product);
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProductbyID(int id)
		{
			var product =  await _cache.GetCacheData<Product>($"Product{id}") ;
			if (product != null)
			{
				return Ok(product);
			}
			var prd = _product.GetProductbyID(id);
			if (prd != null)
			{
				var expiry = DateTime.Now.AddMinutes(5);
				 await _cache.SetCacheData($"Product{id}",prd.Value,expiry);
				return Ok(prd.Value);
			}
			return NotFound();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteProduct(int id)
		{
			var resp = await _product.DeleteProduct(id);
			await _cache.RemoveData($"Product{id}");
			if(resp == null)
			{
				return NotFound();
			}
			return NoContent();

		}
	}
}
