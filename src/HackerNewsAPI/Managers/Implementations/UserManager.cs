using AutoMapper;
using HackerNewsAPI.Managers.Contracts;
using HackerNewsAPI.Models.DTOs;
using HackerNewsAPI.Repositories.Contracts;

namespace HackerNewsAPI.Managers.Implementations;
public class UserManager : IUserManager
{
    private readonly IUserRepository _userRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserManager> _logger;

    public UserManager(IUserRepository userRepository, IItemRepository itemRepository, IMapper mapper, ILogger<UserManager> logger)
    {
        _userRepository = userRepository;
        _itemRepository = itemRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UserDTO?> GetUser(string username)
    {
        try
        {
            var user = await _userRepository.GetUser(username);
            if (user != null)
            {
                var userSubmissions = await GetSubmissionsIdsOf(user.Username);
                user.Submissions = userSubmissions.ToList();
            }
            return _mapper.Map<UserDTO>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting user with id: {username}");
            throw;
        }
    }

    public async Task<IEnumerable<int>> GetSubmissionsIdsOf(string username)
    {
        try
        {
            return await _itemRepository.GetSubmissionsIdsOf(username);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while querying items submissions by: {username}");
            throw;
        }
    }
}