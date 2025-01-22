using MediatR;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.Domain.Model;
using Microsoft.AspNetCore.Authorization;

namespace BlogAPI.Api.Blog;

[Route("Blog")]
public class DeleteBlogController : ApiController
{
    private IMediator _mediator;

    public DeleteBlogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        Task<bool> success = _mediator.Send(new DeleteBlogRequest(id));
        if (success.Result)
        {
            return Ok(new { message = "Blog deleted" });
        }

        return StatusCode(500, new { message = "Blog deleted" });
    }
}