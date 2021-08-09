using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace MetarApplication.Services
{
    public interface IMetarDataService
    {
        string GetMetarData(string scode);
    }

    public class MetarDataService : IMetarDataService
    {
        private IConfiguration _configuration;

        public MetarDataService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetMetarData(string scode)
        {
            string res = String.Empty;
            var url = _configuration.GetSection("metarData:url").Value;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(String.Format("{0}/{1}.TXT",url,scode));
                var responseTask = client.GetAsync("");
                responseTask.Wait();

                var response = responseTask.Result;
                
                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    return null;
                }
            }
            return res;
        }
    }
}