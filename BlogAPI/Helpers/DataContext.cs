using BlogAPI.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Helpers;

public class DataContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }
}