using BlogAPI.Domain.Model;
using MediatR;

namespace BlogAPI.Service.Blog;

public record GetAllBlogsQuery(int Page, int PageSize) : IRequest<BlogsPagedDto>;