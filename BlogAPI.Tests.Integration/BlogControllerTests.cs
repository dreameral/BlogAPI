using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BlogAPI.Domain.Model;
using Xunit;
using FluentAssertions;

namespace BlogAPI.Tests.Integration;

public class BlogControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BlogControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AddBlog_ReturnsOk_WhenRequestIsValid()
    {
        var token = await Login();
        var createBlogDto = new CreateBlogDto(
            null,
            "Test Blog", // Title
            "This is a test blog.", // Content
            "some content" // FriendlyUrl
        );

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsJsonAsync("/Blog", createBlogDto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var blogResponse = await response.Content.ReadFromJsonAsync<BlogDto>();
        blogResponse.Should().NotBeNull();
        blogResponse.Title.Should().Be(createBlogDto.Title);
    }
    
    [Fact]
    public async Task UpdateBlog_ReturnsOk_WhenRequestIsValid()
    {
        var token = await Login();
        var getResponse = await _client.GetAsync("/Blog");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var blogs = await getResponse.Content.ReadFromJsonAsync<BlogsPagedDto>();
        blogs.Blogs.Count.Should().Be(1);
        
        var updatedBlog = new CreateBlogDto(
            null,
            "Test Blog Updated title", // Title
            "This is a test blog.", // Content
            "some content" // FriendlyUrl
        );

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsJsonAsync("/Blog/" + blogs.Blogs.First().Id, updatedBlog);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var blogResponse = await response.Content.ReadFromJsonAsync<BlogDto>();
        blogResponse.Should().NotBeNull();
        blogResponse.Title.Should().Be(updatedBlog.Title);
    }
    
    private async Task<string> Login()
    {
        var loginPayload = new
        {
            username = "admin",
            password = "admin"
        };

        var response = await _client.PostAsJsonAsync("/Login", loginPayload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tokenResponse = await response.Content.ReadAsStringAsync();
        tokenResponse.Should().NotBeNullOrWhiteSpace();

        return tokenResponse;
    }
}