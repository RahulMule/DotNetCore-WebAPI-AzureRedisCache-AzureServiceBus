using DotNetCore_WebAPI_AzureRedisCache.IRepository;
using DotNetCore_WebAPI_AzureRedisCache.Models;
using DotNetCore_WebAPI_AzureRedisCache.ProductContext;

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
	}
}
