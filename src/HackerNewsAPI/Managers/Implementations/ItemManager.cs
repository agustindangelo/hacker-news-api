using AutoMapper;
using HackerNewsAPI.Managers.Contracts;
using HackerNewsAPI.Models;
using HackerNewsAPI.Models.DTOs;
using HackerNewsAPI.Repositories.Contracts;

namespace HackerNewsAPI.Managers.Implementations;
public class ItemManager : IItemManager
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ItemManager> _logger;

    public ItemManager(IItemRepository itemRepository, IMapper mapper, ILogger<ItemManager> logger)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<int>> GetLatestStories()
    {
        try
        {
            return await _itemRepository.GetLatestStories();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting new stories.");
            throw;
        }
    }

    public async Task<ItemDTO?> GetItemById(int id)
    {
        try
        {
            var item = await _itemRepository.GetItemById(id);
            if (item != null)
            {
                var itemKids = await GetKidsIdsOf(id);
                item.Kids = itemKids.ToList();
            }
            return _mapper.Map<ItemDTO>(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting item by id: {id}");
            throw;
        }
    }

    public async Task<IEnumerable<ItemDTO>> SearchStories(string searchInput)
    {
        try
        {
            var items = await _itemRepository.SearchStories(searchInput);
            foreach (Item item in items)
            {
                var itemKids = await GetKidsIdsOf(item.Id);
                item.Kids = itemKids.ToList();
            }
            return _mapper.Map<IEnumerable<ItemDTO>>(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while searching stories with input: {searchInput}");
            throw;
        }
    }

    public async Task<IEnumerable<int>> GetKidsIdsOf(int id)
    {
        try
        {
            return await _itemRepository.GetKidsIdsOf(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting the kids ID for the parent id: {id}");
            throw;
        }
    }
}