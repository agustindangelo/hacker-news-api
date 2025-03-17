using HackerNewsAPI.Controllers;
using HackerNewsAPI.Managers.Contracts;
using HackerNewsAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace HackerNewsAPI.Tests.ControllerTests;
[TestFixture]
public class ItemControllerTests
{
    private Mock<IItemManager> _mockItemManager;
    private MemoryCache _cache;
    private Mock<ILogger<BaseController>> _mockLogger;
    private ItemController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockItemManager = new Mock<IItemManager>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _mockLogger = new Mock<ILogger<BaseController>>();
        _controller = new ItemController(_mockItemManager.Object, _cache, _mockLogger.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _cache.Dispose();
    }

    [Test]
    public async Task GetItemById_CallsItemManager_And_CachesItem_WhenItemNotInCache()
    {
        // Arrange
        var itemId = 1;
        var expectedItem = new ItemDTO { Id = itemId };
        _mockItemManager.Setup(m => m.GetItemById(itemId)).ReturnsAsync(expectedItem);

        // Act
        var result = await _controller.GetItemById(itemId);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnedItem = okResult.Value as ItemDTO;
        Assert.That(returnedItem, Is.Not.Null);
        Assert.That(returnedItem.Id, Is.EqualTo(itemId));

        Assert.That(_cache.TryGetValue($"Item_{itemId}", out ItemDTO? cachedItem), Is.True);
        Assert.That(cachedItem, Is.EqualTo(expectedItem));

        _mockItemManager.Verify(m => m.GetItemById(itemId), Times.Once);
    }

    [Test]
    public async Task GetItemById_ReturnsCachedItem_WhenItemExistsInCache()
    {
        // Arrange
        var itemId = 1;
        var expectedItem = new ItemDTO { Id = itemId };
        _cache.Set($"Item_{itemId}", expectedItem);

        // Act
        var result = await _controller.GetItemById(itemId);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnedItem = okResult.Value as ItemDTO;
        Assert.That(returnedItem, Is.EqualTo(expectedItem));

        _mockItemManager.Verify(m => m.GetItemById(It.IsAny<int>()), Times.Never);
    }

    [Test]
    public async Task GetItemById_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        var itemId = 1;
        ItemDTO? expectedItem = null;
        _mockItemManager.Setup(m => m.GetItemById(itemId)).ReturnsAsync(expectedItem);

        // Act
        var result = await _controller.GetItemById(itemId);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.That(notFoundResult, Is.Not.Null);
        _mockItemManager.Verify(m => m.GetItemById(itemId), Times.Once);
    }

    [Test]
    public async Task SearchStoriesByTitle_ReturnsStories_WhenStoriesExist()
    {
        // Arrange
        var title = "test";
        var expectedStories = new List<ItemDTO> { new ItemDTO { Id = 1, Title = title } };
        _mockItemManager.Setup(m => m.SearchStories(title)).ReturnsAsync(expectedStories);

        // Act
        var result = await _controller.SearchStoriesByTitle(title);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnedStories = okResult.Value as List<ItemDTO>;
        Assert.That(returnedStories, Is.Not.Null);
        Assert.That(returnedStories.Count, Is.EqualTo(1));
        Assert.That(returnedStories[0].Title, Is.EqualTo(title));
    }
}