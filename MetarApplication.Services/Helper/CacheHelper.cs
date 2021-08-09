using System;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json; 
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using MetarApplication.Models;

namespace MetarApplication.Services.Helper
{
	public interface ICacheHelper
	{
		Task<Result> GetMetarData(string scode);

		void SetMetarData(Result result);
	} 

    public class CacheHelper : ICacheHelper
    {
    	private IDistributedCache _distributedCache;
    	private IConfiguration _configuration;

    	public CacheHelper(IDistributedCache distributedCache, IConfiguration configuration)
    	{
    		_distributedCache = distributedCache;
    		_configuration = configuration;
    	}

    	public async Task<Result> GetMetarData(string scode)
    	{
            var redisData = await _distributedCache.GetAsync(scode);
            if (redisData != null)
            {
                string data = Encoding.UTF8.GetString(redisData);
                var result = JsonConvert.DeserializeObject<Result>(data);
                return result;
            }
            return null;
    	}

    	public async void SetMetarData(Result result)
    	{
            var serializedResult = JsonConvert.SerializeObject(result);
            var redisData = Encoding.UTF8.GetBytes(serializedResult);
            var expirationTimeFromConfig = _configuration.GetSection("cacheConfig:expirationTime").Value;
            int expirationTime;
            int.TryParse(expirationTimeFromConfig, out expirationTime);
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(expirationTime));
            await _distributedCache.SetAsync(result.data.station, redisData, options);
    	}
    }
}