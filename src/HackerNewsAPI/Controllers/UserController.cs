using HackerNewsAPI.Managers.Contracts;
using HackerNewsAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : BaseController
{
    private readonly IUserManager _userManager;
    private readonly IMemoryCache _cache;
    private MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromMinutes(10));

    public UserController(IUserManager userManager, IMemoryCache cache, ILogger<BaseController> logger) : base(logger)
    {
        _userManager = userManager;
        _cache = cache;
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetUser(string username)
    {
        try
        {
            var cacheKey = $"User_{username}";
            if (!_cache.TryGetValue(cacheKey, out UserDTO? user))
            {
                user = await _userManager.GetUser(username);
                if (user != null)
                {
                    _cache.Set(cacheKey, user, cacheEntryOptions);
                }
            }
            return HandleSuccess(user);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}