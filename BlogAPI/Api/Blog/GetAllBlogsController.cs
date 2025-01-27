using BlogAPI.Domain.Model;
using BlogAPI.Service.Blog;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Api.Blog;

[Route("Blog")]
public class GetAllBlogsController : ApiController
{
    private IMediator _mediator;

    public GetAllBlogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<BlogsPagedDto> GetAllBlogs(int page = 1, int pageSize = 10)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        return await _mediator.Send(new GetAllBlogsQuery(page, pageSize));
    }
}