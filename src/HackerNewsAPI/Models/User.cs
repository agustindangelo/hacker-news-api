namespace HackerNewsAPI.Models;
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public int Karma { get; set; }
    public string? About { get; set; }
    public List<int> Submissions { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}