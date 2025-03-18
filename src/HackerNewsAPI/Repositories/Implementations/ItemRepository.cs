using System.Data;
using Dapper;
using HackerNewsAPI.Models;
using HackerNewsAPI.Repositories.Contracts;

namespace HackerNewsAPI.Repositories.Implementations;
public class ItemRepository : IItemRepository
{
    private readonly IDbConnection _db;
    private readonly ILogger<ItemRepository> _logger;

    public ItemRepository(IDbConnection db, ILogger<ItemRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Item?> GetItemById(int id)
    {
        try
        {
            var sql = @"
            SELECT Id, Type, ByUsername, Title, Url, Score, Text, Parent
            FROM Items 
            WHERE Id = @Id
            AND Deleted IS NULL AND Dead IS NULL";
            return await _db.QueryFirstOrDefaultAsync<Item>(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting item by id: {id}");
            throw;
        }
    }

    public async Task<IEnumerable<Item>> SearchStories(string searchInput)
    {
        try
        {
            var sql = @"
            SELECT Id, Type, ByUsername, Title, Url, Score
            FROM Items 
            WHERE Type = 'story' 
            AND Title LIKE @SearchValue 
            AND Deleted IS NULL AND Dead IS NULL
            ORDER BY CreatedAt DESC";
            return await _db.QueryAsync<Item>(sql, new { SearchValue = $"%{searchInput}%" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while searching stories with input: {searchInput}");
            throw;
        }
    }

    public async Task<IEnumerable<Item>> GetCommentsByParentId(int parentId)
    {
        try
        {
            var sql = @"
            SELECT Id, Type, ByUsername, Text
            FROM Items 
            WHERE Parent = @ParentId 
            AND Deleted IS NULL AND Dead IS NULL
            ORDER BY CreatedAt";
            return await _db.QueryAsync<Item>(sql, new { ParentId = parentId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting comments by parent id: {parentId}");
            throw;
        }
    }

    public async Task<IEnumerable<int>> GetKidsIdsOf(int parentId)
    {
        try
        {
            var sql = @"
            SELECT Id 
            FROM Items 
            WHERE Parent = @ParentId
            AND Deleted IS NULL AND Dead IS NULL
            ORDER BY CreatedAt DESC";
            return await _db.QueryAsync<int>(sql, new { ParentId = parentId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while querying kid items for parent id: {parentId}");
            throw;
        }
    }

    public async Task<IEnumerable<int>> GetLatestStories()
    {
        try
        {
            var sql = @"
            SELECT Id 
            FROM Items 
            WHERE Type = 'story' 
            AND Deleted IS NULL AND Dead IS NULL
            ORDER BY CreatedAt DESC";
            return await _db.QueryAsync<int>(sql);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting new stories");
            throw;
        }
    }

    public async Task<IEnumerable<int>> GetSubmissionsIdsOf(string username)
    {
        try
        {
            var sql = @"
            SELECT Id
            FROM Items 
            WHERE ByUsername = @ByUsername
            AND Deleted IS NULL AND Dead IS NULL
            ORDER BY CreatedAt DESC";
            return await _db.QueryAsync<int>(sql, new { ByUsername = username });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while querying items submissions by: {username}");
            throw;
        }
    }
}