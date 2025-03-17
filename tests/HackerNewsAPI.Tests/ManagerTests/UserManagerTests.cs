using AutoMapper;
using HackerNewsAPI.Managers.Implementations;
using HackerNewsAPI.Models;
using HackerNewsAPI.Models.DTOs;
using HackerNewsAPI.Repositories.Contracts;
using Microsoft.Extensions.Logging;
using Moq;

namespace HackerNewsAPI.Tests.ManagerTests;

[TestFixture]
public class UserManagerTests
{
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IItemRepository> _mockItemRepository;
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<UserManager>> _mockLogger;
    private UserManager _userManager;

    [SetUp]
    public void SetUp()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockItemRepository = new Mock<IItemRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<UserManager>>();
        _userManager = new UserManager(_mockUserRepository.Object, _mockItemRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetUser_ReturnsUserDTO_WhenUserExists()
    {
        // Arrange
        var username = "testuser";
        var user = new User { Username = username };
        var expectedUserDTO = new UserDTO { Username = username, Submitted = new List<int> { 1, 2, 3 } };

        _mockUserRepository.Setup(r => r.GetUser(username)).ReturnsAsync(user);
        _mockItemRepository.Setup(r => r.GetSubmissionsIdsOf(username)).ReturnsAsync(new List<int> { 1, 2, 3 });
        _mockMapper.Setup(m => m.Map<UserDTO>(user)).Returns(expectedUserDTO);

        // Act
        var result = await _userManager.GetUser(username);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo(username));
        Assert.That(result.Submitted, Is.EquivalentTo(new List<int> { 1, 2, 3 }));

        _mockUserRepository.Verify(r => r.GetUser(username), Times.Once);
        _mockItemRepository.Verify(r => r.GetSubmissionsIdsOf(username), Times.Once);
        _mockMapper.Verify(m => m.Map<UserDTO>(user), Times.Once);
    }

    [Test]
    public async Task GetUser_ReturnsNull_WhenUserDoesNotExist()
    {
        // Arrange
        var username = "unknownuser";
        _mockUserRepository.Setup(r => r.GetUser(username)).ReturnsAsync((User?)null);

        // Act
        var result = await _userManager.GetUser(username);

        // Assert
        Assert.That(result, Is.Null);

        _mockUserRepository.Verify(r => r.GetUser(username), Times.Once);
        _mockItemRepository.Verify(r => r.GetSubmissionsIdsOf(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetUser_ThrowsException_WhenRepositoryThrowsException()
    {
        // Arrange
        var username = "testuser";
        _mockUserRepository.Setup(r => r.GetUser(username)).ThrowsAsync(new Exception("Repository error"));

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _userManager.GetUser(username));
    }

    [Test]
    public async Task GetSubmissionsIdsOf_ReturnsSubmissionsIds_WhenSubmissionsExist()
    {
        // Arrange
        var username = "testuser";
        var expectedSubmissionsIds = new List<int> { 1, 2, 3 };
        _mockItemRepository.Setup(r => r.GetSubmissionsIdsOf(username)).ReturnsAsync(expectedSubmissionsIds);

        // Act
        var result = await _userManager.GetSubmissionsIdsOf(username);

        // Assert
        Assert.That(result, Is.EqualTo(expectedSubmissionsIds));
    }

    [Test]
    public void GetSubmissionsIdsOf_ThrowsException_WhenRepositoryThrowsException()
    {
        // Arrange
        var username = "testuser";
        _mockItemRepository.Setup(r => r.GetSubmissionsIdsOf(username)).ThrowsAsync(new Exception("Repository error"));

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _userManager.GetSubmissionsIdsOf(username));
    }
}