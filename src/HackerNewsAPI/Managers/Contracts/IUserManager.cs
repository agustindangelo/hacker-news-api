using HackerNewsAPI.Models.DTOs;
namespace HackerNewsAPI.Managers.Contracts;
public interface IUserManager
{
    Task<UserDTO?> GetUser(string username);
    Task<IEnumerable<int>> GetSubmissionsIdsOf(string username);
}