using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Hv.Ppb302.DigitalThesis.WebClient.ViewComponents;

public class SizeGuardViewComponent(PageRepository pageRepo, IMemoryCache cache) : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync()
    {
        var cacheKey = "SizeGuardData";
        if (!cache.TryGetValue(cacheKey, out Page data))
        {
            // Key not in cache, so get data from database
            data = pageRepo.GetByName("SizeGuard");

            // Set cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5), // Cache for 5 minutes
                SlidingExpiration = TimeSpan.FromMinutes(2) // Reset expiration if accessed within this time
            };

            // Save data in cache
            cache.Set(cacheKey, data, cacheEntryOptions);
        }

        return Task.FromResult<IViewComponentResult>(View(data));
    }
}
