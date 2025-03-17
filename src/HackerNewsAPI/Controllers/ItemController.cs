using HackerNewsAPI.Managers.Contracts;
using HackerNewsAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ItemController : BaseController
{
    private readonly IItemManager _itemManager;
    private readonly IMemoryCache _cache;
    private MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromMinutes(10));

    public ItemController(IItemManager itemManager, IMemoryCache cache, ILogger<BaseController> logger) : base(logger)
    {
        _itemManager = itemManager;
        _cache = cache;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItemById(int id)
    {
        try
        {
            var cacheKey = $"Item_{id}";
            if (!_cache.TryGetValue(cacheKey, out ItemDTO? item))
            {
                item = await _itemManager.GetItemById(id);
                if (item != null)
                {
                    _cache.Set(cacheKey, item, cacheEntryOptions);
                }
            }
            return HandleSuccess(item);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchStoriesByTitle([FromQuery] string title)
    {
        try
        {
            var cacheKey = $"SearchStories_{title}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ItemDTO>? stories))
            {
                stories = await _itemManager.SearchStories(title);
                if (stories != null)
                {
                    _cache.Set(cacheKey, stories, cacheEntryOptions);
                }
            }
            return HandleSuccess(stories);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}