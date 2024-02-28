using DotNetCore_WebAPI_AzureRedisCache.Models;

namespace DotNetCore_WebAPI_AzureRedisCache.IRepository
{
	public interface ICache
	{
		public Task<T> GetCacheData<T>(String key);
		public Task<Object> RemoveData(String key);
		public Task<bool> SetCacheData<T>(String key, T value, DateTimeOffset expirationtime);

	}
}
