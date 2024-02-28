using DotNetCore_WebAPI_AzureRedisCache.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore_WebAPI_AzureRedisCache.IRepository
{
	public interface IProduct
	{
		public List<Product> GetProducts();
		public Product AddProduct(Product product);
		public ActionResult<Product> GetProductbyID(int id);
		public Task<ActionResult> DeleteProduct(int id);
	}
}
