using HackerNews.Api.Models.DTOs;
namespace HackerNews.Api.Managers.Contracts;
public interface IUserManager
{
    Task<UserDTO?> GetUser(string username);
    Task<IEnumerable<int>> GetSubmissionsIdsOf(string username);
}