using BlogAPI.Domain.Entity;
using BlogAPI.Domain.Model;
using BlogAPI.Helpers;
using BlogAPI.Service.Blog;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Tests.Unit;

using System.Collections.Generic;
using Moq;
using Xunit;

public class BlogServiceTests
{
    [Fact]
    public void Test_TitleAlreadyExists()
    {
        var mockDb = new Mock<DataContext>();
        var mockSet = new Mock<DbSet<Blog>>();
        var blogs = new List<Blog>
        {
            new Blog
            {
                Title = "Existing Title",
                Content = "Some content",
                FriendlyUrl = "http://some url",
                DateCreated = DateTime.UtcNow
            }
        }.AsQueryable();
        mockSet.As<IQueryable<Blog>>().Setup(m => m.Provider).Returns(blogs.Provider);
        mockSet.As<IQueryable<Blog>>().Setup(m => m.Expression).Returns(blogs.Expression);
        mockSet.As<IQueryable<Blog>>().Setup(m => m.ElementType).Returns(blogs.ElementType);
        mockSet.As<IQueryable<Blog>>().Setup(m => m.GetEnumerator()).Returns(() => blogs.GetEnumerator());
        mockDb.Setup(m => m.Blogs).Returns(mockSet.Object);

        var validator = new CreateBlogValidator(mockDb.Object);
        var result = validator.ValidateAsync(new CreateBlogDto(null, "Existing Title", "http://test.com", "Some content"));
        
        Assert.False(result.Result.IsValid);
    }
}
