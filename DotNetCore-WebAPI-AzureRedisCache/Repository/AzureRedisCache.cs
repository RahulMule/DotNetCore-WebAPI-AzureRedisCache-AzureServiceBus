using DotNetCore_WebAPI_AzureRedisCache.IRepository;
using DotNetCore_WebAPI_AzureRedisCache.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DotNetCore_WebAPI_AzureRedisCache.Repository
{
	public class AzureRedisCache : ICache
	{
		private IDatabase _database;
		public AzureRedisCache(IConnectionMultiplexer connectionMultiplexer) {
		_database = connectionMultiplexer.GetDatabase();
		}
		async Task<T> ICache.GetCacheData<T>(string key)
		{
			var value = await _database.StringGetAsync(key);
			if (!String.IsNullOrEmpty(value))
			{
				return JsonConvert.DeserializeObject<T>(value);
			}
			return default;
		}

		Task<object> ICache.RemoveData(string key)
		{
			throw new NotImplementedException();
		}

		 async Task<bool> ICache.SetCacheData<T>(string key, T value, DateTimeOffset expirationtime)
		{
			TimeSpan expirytime = expirationtime.DateTime.Subtract(DateTime.Now);
			var isSet = await _database.StringSetAsync(key,JsonConvert.SerializeObject(value),expirytime);
			return isSet;
		}
	}
}
