using System.Data;
using Dapper;
using HackerNewsAPI.Models;
using HackerNewsAPI.Repositories.Contracts;

namespace HackerNewsAPI.Repositories.Implementations;
public class UserRepository : IUserRepository
{
    private readonly IDbConnection _db;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(IDbConnection db, ILogger<UserRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<User?> GetUser(string username)
    {
        try
        {
            var sql = @"
            SELECT Id, Username, Karma, About, CreatedAt 
            FROM Users 
            WHERE Username = @Username";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting user: {username}");
            throw;
        }
    }
}