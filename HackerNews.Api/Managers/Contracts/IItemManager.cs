using HackerNews.Api.Models.DTOs;
namespace HackerNews.Api.Managers.Contracts;
public interface IItemManager
{
    Task<IEnumerable<int>> GetLatestStories();
    Task<ItemDTO?> GetItemById(int id);
    Task<IEnumerable<ItemDTO>> SearchStories(string searchInput);
    Task<IEnumerable<int>> GetKidsIdsOf(int id);
}