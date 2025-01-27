using BlogAPI.Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Api.Blog;

[Route("Blog")]
public class SaveBlogController : ApiController
{
    private IMediator _mediator;
    private readonly IValidator<CreateBlogDto> _createBlogValidator;

    public SaveBlogController(IMediator mediator, IValidator<CreateBlogDto> createBlogValidator)
    {
        _mediator = mediator;
        _createBlogValidator = createBlogValidator;
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> AddBlog([FromBody] CreateBlogDto requestDto)
    {
        var validationResult = await _createBlogValidator.ValidateAsync(requestDto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        return Ok(await _mediator.Send(requestDto));
    }
    
    [Authorize(Roles = "ADMIN")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlog(string id, [FromBody] CreateBlogDto requestDto)
    {
        var validationResult = await _createBlogValidator.ValidateAsync(requestDto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var updatedRequestDto = requestDto with { Id = id };
        return Ok(await _mediator.Send(updatedRequestDto));
    }
}