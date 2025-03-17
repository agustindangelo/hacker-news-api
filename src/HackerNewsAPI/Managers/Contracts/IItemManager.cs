using HackerNewsAPI.Models.DTOs;
namespace HackerNewsAPI.Managers.Contracts;
public interface IItemManager
{
    Task<IEnumerable<int>> GetNewestStories();
    Task<ItemDTO?> GetItemById(int id);
    Task<IEnumerable<ItemDTO>> SearchStories(string searchInput);
    Task<IEnumerable<int>> GetKidsIdsOf(int id);
}