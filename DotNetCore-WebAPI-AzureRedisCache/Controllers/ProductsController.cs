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
			if( products != null)
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
	}
}
