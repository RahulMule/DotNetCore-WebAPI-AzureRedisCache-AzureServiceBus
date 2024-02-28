using DotNetCore_WebAPI_AzureRedisCache.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore_WebAPI_AzureRedisCache.ProductContext
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<Product> Products { get; set; }
	}
}
