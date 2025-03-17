using HackerNewsAPI.Models;
namespace HackerNewsAPI.Repositories.Contracts;
public interface IUserRepository
{
    Task<User?> GetUser(string username);
}