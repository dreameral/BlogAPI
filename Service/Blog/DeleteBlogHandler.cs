using BlogAPI.Domain.Model;
using BlogAPI.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogAPI.Service.Blog;

public class DeleteBlogHandler : IRequestHandler<DeleteBlogRequest, bool>
{
    
    private readonly DataContext _context;

    public DeleteBlogHandler(DataContext context)
    {
        _context = context;
    }
    
    public Task<bool> Handle(DeleteBlogRequest request, CancellationToken cancellationToken)
    {
        Domain.Entity.Blog? blog = _context.Blogs.Find(request.Id);

        if (blog is not null)
        {
            _context.Blogs.Remove(blog);
            _context.SaveChanges();   
        }
        return Task.FromResult(true);
    }
}