using HackerNewsAPI.Models;
namespace HackerNewsAPI.Repositories.Contracts;
public interface IItemRepository
{
    Task<Item?> GetItemById(int id);
    Task<IEnumerable<Item>> SearchStories(string searchInput);
    Task<IEnumerable<Item>> GetCommentsByParentId(int parentId);
    Task<IEnumerable<int>> GetKidsIdsOf(int parentId);
    Task<IEnumerable<int>> GetLatestStories();
    Task<IEnumerable<int>> GetSubmissionsIdsOf(string username);
}