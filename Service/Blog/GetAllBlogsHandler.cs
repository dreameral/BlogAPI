using BlogAPI.Domain.Model;
using BlogAPI.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Service.Blog;

public class GetAllBlogsHandler : IRequestHandler<GetAllBlogsQuery, BlogsPagedDto>
{
    private readonly DataContext _context;

    public GetAllBlogsHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<BlogsPagedDto> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
    {
        var total = await _context.Blogs.CountAsync(cancellationToken);
        var blogs = await _context.Blogs
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(b => new BlogDto(b.Id, b.Title, b.FriendlyUrl, b.Content, b.DateCreated))
            .ToListAsync(cancellationToken);
        
        return new BlogsPagedDto(blogs, request.Page, blogs.Count, total);
    }
}