using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Hv.Ppb302.DigitalThesis.WebClient.ViewComponents;

public class SizeGuardViewComponent : ViewComponent
{
    private readonly PageRepository _pageRepo;
    private readonly IMemoryCache _cache;

    public SizeGuardViewComponent(PageRepository pageRepo, IMemoryCache cache)
    {
        _pageRepo = pageRepo;
        _cache = cache;
    }

    public Task<IViewComponentResult> InvokeAsync()
    {
        var cacheKey = "SizeGuardData";
        if (!_cache.TryGetValue(cacheKey, out Page data))
        {
            // Key not in cache, so get data from database
            data = _pageRepo.GetByName("SizeGuard");

            // Set cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5), // Cache for 5 minutes
                SlidingExpiration = TimeSpan.FromMinutes(2) // Reset expiration if accessed within this time
            };

            // Save data in cache
            _cache.Set(cacheKey, data, cacheEntryOptions);
        }

        return Task.FromResult<IViewComponentResult>(View(data));
    }
}
