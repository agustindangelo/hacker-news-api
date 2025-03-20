namespace HackerNews.Api.Models;
public class Comment
{
    public int Id { get; set; }  
    public int UserId { get; set; }  
    public int StoryId { get; set; }  
    public int? ParentCommentId { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}