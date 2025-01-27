using BlogAPI.Domain.Model;
using BlogAPI.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Service.Blog;

public class SaveBlogHandler : IRequestHandler<CreateBlogDto, BlogDto>
{
    private readonly DataContext _context;

    public SaveBlogHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<BlogDto> Handle(CreateBlogDto request, CancellationToken cancellationToken)
    {
        Domain.Entity.Blog blog;

        switch (request.Id)
        {
            case null: // Insert
                blog = new Domain.Entity.Blog
                {
                    Title = request.Title,
                    Content = request.Content,
                    FriendlyUrl = request.FriendlyUrl,
                    DateCreated = DateTime.UtcNow,
                };
                _context.Blogs.Add(blog);
                await _context.SaveChangesAsync(cancellationToken); // Save to generate the ID
                break;

            default: // Update
                blog = _context.Blogs.Find(request.Id) ?? throw new KeyNotFoundException("Blog not found");
                _context.Entry(blog).State = EntityState.Detached;
                blog = blog with
                {
                    Title = request.Title,
                    Content = request.Content,
                    FriendlyUrl = request.FriendlyUrl,
                };
                _context.Blogs.Update(blog);
                await _context.SaveChangesAsync(cancellationToken);
                break;
        }

        // Retrieve the record using the ID (for both insert and update scenarios)
        return await _context.Blogs
            .Where(b => b.Id == blog.Id) // Use the blog's ID
            .Select(b => new BlogDto(b.Id, b.Title, b.Content, b.FriendlyUrl, b.DateCreated))
            .FirstAsync(cancellationToken);
    }
}
