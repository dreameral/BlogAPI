using MediatR;

namespace BlogAPI.Domain.Model;

public record DeleteBlogRequest(string Id) : IRequest<bool>;