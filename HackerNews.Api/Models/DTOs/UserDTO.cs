namespace HackerNews.Api.Models.DTOs;
public class UserDTO
{
    public string Username { get; set; } = string.Empty;
    public string? About { get; set; }
    public long Created { get; set; }
    public string Id { get; set; } = string.Empty;
    public int Karma { get; set; }
    public IEnumerable<int> Submitted { get; set; } = [];
}