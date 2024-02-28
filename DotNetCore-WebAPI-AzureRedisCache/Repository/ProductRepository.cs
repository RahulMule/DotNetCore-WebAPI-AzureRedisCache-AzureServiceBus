using DotNetCore_WebAPI_AzureRedisCache.IRepository;
using DotNetCore_WebAPI_AzureRedisCache.Models;
using DotNetCore_WebAPI_AzureRedisCache.ProductContext;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore_WebAPI_AzureRedisCache.Repository
{
	public class ProductRepository : IProduct
	{
		AppDbContext _context;
		public ProductRepository(AppDbContext context) { 
		_context = context;
		}
		public List<Product> GetProducts()
		{
			var products = _context.Products.ToList();
			return products;
		}

		public Product AddProduct(Product product)
		{
			 _context.Products.Add(product);
			_context.SaveChanges();
			return product;
		}

		

		ActionResult<Product> IProduct.GetProductbyID(int id)
		{
			var product = _context.Products.FirstOrDefault(x => x.Id == id);
			if (product == null)
			{
				return default;
			}
			return product;
		}

		async Task<ActionResult> IProduct.DeleteProduct(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null)
			{
				return null;
			}
			_context.Products.Remove(product);
			await _context.SaveChangesAsync();
			return new NoContentResult();

		}
	}
}
