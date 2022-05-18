using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using ObjectCaching.Extensions;
using ObjectCaching.Interface;
using ObjectCaching.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjectCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IEfRepository<City> _cityRepository;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        public CityController(IEfRepository<City> cityRepository, IDistributedCache cache, IConfiguration configuration)
        {
            _cityRepository = cityRepository;
            _cache = cache;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string recordKey = "CitiesList_" + DateTime.Now.ToString("yyyyMMdd_hhmm");
            var cities = await _cache.GetRecordAsync<List<City>>(recordKey);

            if (cities is null)
            {
                cities = (List<City>)await _cityRepository.ListAllAsync();

                // Add CacheItem to cache
                await _cache.SetRecordAsync(recordKey, cities);
            }

            return Ok(cities);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            ICache _cache = CacheManager.GetCache(_configuration.GetValue<string>("NCacheSettings:CacheName")); //connect to cache in NCache
            var cacheKey = string.Format("city_{0}", id);

            var city = _cache.Get<City>(cacheKey);

            if (city is null)
            {
                city = await _cityRepository.GetByIdAsync(id);

                var cacheItem = new CacheItem(city)
                {
                    Expiration = new Expiration(ExpirationType.Sliding, TimeSpan.FromMinutes(10))
                };

                // Add CacheItem to cache
                await _cache.InsertAsync(cacheKey, cacheItem);
            }

            return Ok(city);
        }
    }
}
