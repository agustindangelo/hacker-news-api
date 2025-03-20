using HackerNews.Api.Models;
namespace HackerNews.Api.Repositories.Contracts;
public interface IUserRepository
{
    Task<User?> GetUser(string username);
}