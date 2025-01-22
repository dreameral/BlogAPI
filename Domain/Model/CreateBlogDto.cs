using MediatR;

namespace BlogAPI.Domain.Model;

public record CreateBlogDto(
    string? Id,
    string Title,
    string FriendlyUrl,
    string Content) : IRequest<BlogDto>;