using DotNetCore_WebAPI_AzureRedisCache.IRepository;
using DotNetCore_WebAPI_AzureRedisCache.Models;
using DotNetCore_WebAPI_AzureRedisCache.Repository;
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
		public ProductsController(IProduct product, ICache cache)
		{
			_product = product;
			_cache = cache;
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
		public Product AddProducts(Product product)
		{
			return _product.AddProduct(product);
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
