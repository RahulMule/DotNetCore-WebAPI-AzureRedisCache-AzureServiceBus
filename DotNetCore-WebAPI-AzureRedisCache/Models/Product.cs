using System.ComponentModel.DataAnnotations;

namespace DotNetCore_WebAPI_AzureRedisCache.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(50)]
		public string Name { get; set; }
		[Required]
		[StringLength(150)]
		public string Description { get; set; }

		[Required]
		public decimal price { get; set; }
	}
}
