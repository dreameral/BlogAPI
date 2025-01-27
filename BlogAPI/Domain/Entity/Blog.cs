using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Domain.Entity;

public record Blog
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; init; } = default!;

    public string Title { get; init; } = default!;
    public string FriendlyUrl { get; init; } = default!;
    public string Content { get; init; } = default!;
    public DateTime DateCreated { get; init; }
}