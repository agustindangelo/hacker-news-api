using HackerNews.Api.Controllers;
using HackerNews.Api.Managers.Contracts;
using HackerNews.Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace HackerNews.Api.Tests.ControllerTests;
[TestFixture]
public class UserControllerTests
{
    private Mock<IUserManager> _mockUserManager;
    private MemoryCache _cache;
    private Mock<ILogger<BaseController>> _mockLogger;
    private UserController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockUserManager = new Mock<IUserManager>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _mockLogger = new Mock<ILogger<BaseController>>();
        _controller = new UserController(_mockUserManager.Object, _cache, _mockLogger.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _cache.Dispose();
    }

    [Test]
    public async Task GetUser_CallsUserManager_And_CachesUser_WhenUserNotInCache()
    {
        // Arrange
        var username = "testuser";
        var expectedUser = new UserDTO { Username = username };
        _mockUserManager.Setup(m => m.GetUser(username)).ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.GetUser(username);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnedUser = okResult.Value as UserDTO;
        Assert.That(returnedUser, Is.EqualTo(expectedUser));

        Assert.That(_cache.TryGetValue($"User_{username}", out UserDTO? cachedUser), Is.True);
        Assert.That(cachedUser, Is.EqualTo(expectedUser));

        _mockUserManager.Verify(m => m.GetUser(username), Times.Once);
    }

    [Test]
    public async Task GetUser_ReturnsCachedUser_WhenUserExistsInCache()
    {
        // Arrange
        var username = "testuser";
        var expectedUser = new UserDTO { Username = username };
        _cache.Set($"User_{username}", expectedUser);

        // Act
        var result = await _controller.GetUser(username);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnedUser = okResult.Value as UserDTO;
        Assert.That(returnedUser, Is.EqualTo(expectedUser));

        _mockUserManager.Verify(m => m.GetUser(It.IsAny<string>()), Times.Never, "UserManager should not be called if cache has the user.");
    }

    [Test]
    public async Task GetUser_CallsUserManager_WhenUserNotInCache()
    {
        // Arrange
        var username = "testuser";
        var expectedUser = new UserDTO { Username = username };

        _mockUserManager.Setup(m => m.GetUser(username)).ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.GetUser(username);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnedUser = okResult.Value as UserDTO;
        Assert.That(returnedUser, Is.EqualTo(expectedUser));

        _mockUserManager.Verify(m => m.GetUser(username), Times.Once);
        Assert.That(_cache.TryGetValue($"User_{username}", out UserDTO? cachedUser), Is.True);
        Assert.That(cachedUser, Is.EqualTo(expectedUser));
    }

    [Test]
    public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var username = "unknownuser";

        _mockUserManager.Setup(m => m.GetUser(username)).ReturnsAsync((UserDTO?)null);

        // Act
        var result = await _controller.GetUser(username);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.That(notFoundResult, Is.Not.Null);
        _mockUserManager.Verify(m => m.GetUser(username), Times.Once);
    }

    [Test]
    public async Task GetUser_HandlesException_ReturnsError()
    {
        // Arrange
        var username = "erroruser";

        _mockUserManager.Setup(m => m.GetUser(username)).ThrowsAsync(new Exception("Database failure"));

        // Act
        var result = await _controller.GetUser(username);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.That(objectResult, Is.Not.Null);
        Assert.That(objectResult.StatusCode, Is.EqualTo(500));

        _mockUserManager.Verify(m => m.GetUser(username), Times.Once);
    }
}