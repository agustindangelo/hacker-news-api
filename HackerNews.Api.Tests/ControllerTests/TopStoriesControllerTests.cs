using HackerNews.Api.Controllers;
using HackerNews.Api.Managers.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace HackerNews.Api.Tests.ControllerTests;

[TestFixture]
public class TopStoriesControllerTests
{
    private Mock<IItemManager> _mockItemManager;
    private MemoryCache _cache;
    private Mock<ILogger<BaseController>> _mockLogger;
    private TopStoriesController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockItemManager = new Mock<IItemManager>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _mockLogger = new Mock<ILogger<BaseController>>();
        _controller = new TopStoriesController(_mockItemManager.Object, _cache, _mockLogger.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _cache.Dispose();
    }

    [Test]
    public async Task GetLatestStories_CallsItemManager_And_CachesResult_WhenStoriesNotInCache()
    {
        // Arrange
        var expectedStories = new List<int> { 1, 2, 3 };
        _mockItemManager.Setup(m => m.GetLatestStories()).ReturnsAsync(expectedStories);

        // Act
        var result = await _controller.GetLatestStories();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnedStories = okResult.Value as List<int>;
        Assert.That(returnedStories, Is.Not.Null);
        Assert.That(returnedStories.Count, Is.EqualTo(3));
        Assert.That(returnedStories, Is.EqualTo(expectedStories));

        Assert.That(_cache.TryGetValue($"TopStories", out List<int>? cachedIds), Is.True);
        Assert.That(cachedIds, Is.EqualTo(expectedStories));

        _mockItemManager.Verify(m => m.GetLatestStories(), Times.Once);
    }

    [Test]
    public async Task GetLatestStories_ReturnsCachedIds_WhenStoriesIdsExistsInCache()
    {
        // Arrange
        var expectedStories = new List<int> { 1, 2, 3 };
        _mockItemManager.Setup(m => m.GetLatestStories()).ReturnsAsync(expectedStories);
        _cache.Set($"TopStories", expectedStories);

        // Act
        var result = await _controller.GetLatestStories();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnedStories = okResult.Value as List<int>;
        Assert.That(returnedStories, Is.EqualTo(expectedStories));

        _mockItemManager.Verify(m => m.GetLatestStories(), Times.Never);
    }
}