using AutoMapper;
using HackerNews.Api.Managers.Implementations;
using HackerNews.Api.Models;
using HackerNews.Api.Models.DTOs;
using HackerNews.Api.Repositories.Contracts;
using Microsoft.Extensions.Logging;
using Moq;

namespace HackerNews.Api.Tests.ManagerTests;

[TestFixture]
public class ItemManagerTests
{
    private Mock<IItemRepository> _mockItemRepository;
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<ItemManager>> _mockLogger;
    private ItemManager _itemManager;

    [SetUp]
    public void SetUp()
    {
        _mockItemRepository = new Mock<IItemRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<ItemManager>>();
        _itemManager = new ItemManager(_mockItemRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetLatestStories_ReturnsStories_WhenStoriesExist()
    {
        // Arrange
        var expectedStories = new List<int> { 1, 2, 3 };
        _mockItemRepository.Setup(r => r.GetLatestStories()).ReturnsAsync(expectedStories);

        // Act
        var result = await _itemManager.GetLatestStories();

        // Assert
        Assert.That(result, Is.EqualTo(expectedStories));
    }

    [Test]
    public void GetLatestStories_ThrowsException_WhenRepositoryThrowsException()
    {
        // Arrange
        _mockItemRepository.Setup(r => r.GetLatestStories()).ThrowsAsync(new Exception("Repository error"));

        // Act and Assert
        Assert.ThrowsAsync<Exception>(_itemManager.GetLatestStories);
    }

    [Test]
    public async Task GetItemById_ReturnsItem_WhenItemExists()
    {
        // Arrange
        var itemId = 1;
        var item = new Item { Id = itemId, Kids = new List<int>() };
        var expectedItemDTO = new ItemDTO { Id = itemId };
        _mockItemRepository.Setup(r => r.GetItemById(itemId)).ReturnsAsync(item);
        _mockItemRepository.Setup(r => r.GetKidsIdsOf(itemId)).ReturnsAsync(new List<int>());
        _mockMapper.Setup(m => m.Map<ItemDTO>(item)).Returns(expectedItemDTO);

        // Act
        var result = await _itemManager.GetItemById(itemId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedItemDTO));
    }

    [Test]
    public void GetItemById_ThrowsException_WhenRepositoryThrowsException()
    {
        // Arrange
        var itemId = 1;
        _mockItemRepository.Setup(r => r.GetItemById(itemId)).ThrowsAsync(new Exception("Repository error"));

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _itemManager.GetItemById(itemId));
    }

    [Test]
    public async Task SearchStories_ReturnsStories_WhenStoriesExist()
    {
        // Arrange
        var searchInput = "test";
        var items = new List<Item> { new Item { Id = 1, Title = "test story", Kids = new List<int>() } };
        var expectedItemsDTO = new List<ItemDTO> { new ItemDTO { Id = 1, Title = "test story" } };
        _mockItemRepository.Setup(r => r.SearchStories(searchInput)).ReturnsAsync(items);
        _mockItemRepository.Setup(r => r.GetKidsIdsOf(It.IsAny<int>())).ReturnsAsync(new List<int>());
        _mockMapper.Setup(m => m.Map<IEnumerable<ItemDTO>>(items)).Returns(expectedItemsDTO);

        // Act
        var result = await _itemManager.SearchStories(searchInput);

        // Assert
        Assert.That(result, Is.EqualTo(expectedItemsDTO));
    }

    [Test]
    public void SearchStories_ThrowsException_WhenRepositoryThrowsException()
    {
        // Arrange
        var searchInput = "test";
        _mockItemRepository.Setup(r => r.SearchStories(searchInput)).ThrowsAsync(new Exception("Repository error"));

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _itemManager.SearchStories(searchInput));
    }

    [Test]
    public async Task GetKidsIdsOf_ReturnsKidsIds_WhenKidsExist()
    {
        // Arrange
        var parentId = 1;
        var expectedKidsIds = new List<int> { 2, 3 };
        _mockItemRepository.Setup(r => r.GetKidsIdsOf(parentId)).ReturnsAsync(expectedKidsIds);

        // Act
        var result = await _itemManager.GetKidsIdsOf(parentId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedKidsIds));
    }

    [Test]
    public void GetKidsIdsOf_ThrowsException_WhenRepositoryThrowsException()
    {
        // Arrange
        var parentId = 1;
        _mockItemRepository.Setup(r => r.GetKidsIdsOf(parentId)).ThrowsAsync(new Exception("Repository error"));

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _itemManager.GetKidsIdsOf(parentId));
    }
}