using HackerNewsAPI.Managers.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsAPI.Controllers;
[Route("api/topstories")]
[ApiController]
public class TopStoriesController : BaseController
{
    private readonly IItemManager _itemManager;
    private readonly IMemoryCache _cache;

    public TopStoriesController(IItemManager itemManager, IMemoryCache cache, ILogger<BaseController> logger) : base(logger)
    {
        _itemManager = itemManager;
        _cache = cache;
    }

    [HttpGet]
    public async Task<IActionResult> GetTopStories()
    {
        try
        {
            var cacheKey = "TopStories";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<int>? storiesIds))
            {
                storiesIds = await _itemManager.GetNewestStories();
                if (storiesIds != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    _cache.Set(cacheKey, storiesIds, cacheEntryOptions);
                }
            }
            return HandleSuccess(storiesIds);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}