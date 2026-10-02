using Microsoft.AspNetCore.Http;

namespace SocialMedia.Application.Posts;

public class CreatePostRequest
{
    public string? Content { get; set; }
    public IFormFile? Image { get; set; }
    public IFormFile? Video { get; set; }
}

public class UpdatePostRequest
{
    public string? Content { get; set; }
    public IFormFile? Image { get; set; }
    public IFormFile? Video { get; set; }
    public bool RemoveImage { get; set; }
    public bool RemoveVideo { get; set; }
}

public class PostResponse
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? ProfileImageUrl { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
}