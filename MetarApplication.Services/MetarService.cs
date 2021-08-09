using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using MetarApplication.Models;
using MetarApplication.Services.Helper;
using MetarApplication.Services.Mapper;

namespace MetarApplication.Services
{
    public interface IMetarService
	{
		Result GetData(Headers headers, string scode);
	}

    public class MetarService : IMetarService
    {
    	private IResponseMapper _responseMapper;
    	private ICacheHelper _cacheHelper;
    	private IMetarDataService _metarDataService;

    	public MetarService(IResponseMapper responseMapper, ICacheHelper cacheHelper, IMetarDataService metarDataService)
    	{
    		_responseMapper = responseMapper;
    		_cacheHelper = cacheHelper;
    		_metarDataService = metarDataService;
    	}

    	public Result GetData(Headers headers, string scode)
    	{
    		if(headers.nocache!=1)
    		{
	    		var resultFromCache = _cacheHelper.GetMetarData(scode);
	    		if(resultFromCache.Result!=null)
	    		 return resultFromCache.Result;
	    	}	
    		var metarData = _metarDataService.GetMetarData(scode);
    		if(string.IsNullOrEmpty(metarData))
                return null;
            var result = _responseMapper.MapResponse(metarData);
            _cacheHelper.SetMetarData(result);
            return result;
    	}
    }
}
