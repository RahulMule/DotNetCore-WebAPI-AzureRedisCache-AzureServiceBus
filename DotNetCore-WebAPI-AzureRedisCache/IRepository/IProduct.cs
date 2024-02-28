using DotNetCore_WebAPI_AzureRedisCache.Models;

namespace DotNetCore_WebAPI_AzureRedisCache.IRepository
{
	public interface IProduct
	{
		public List<Product> GetProducts();
		public Product AddProduct(Product product);
	}
}
