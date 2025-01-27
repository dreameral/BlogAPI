using BlogAPI.Domain.Model;
using BlogAPI.Helpers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Service.Blog;

public class CreateBlogValidator : AbstractValidator<CreateBlogDto>
{

    public CreateBlogValidator(DataContext context)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(1000).WithMessage("Content cannot exceed 1000 characters.");

        RuleFor(x => x.FriendlyUrl)
            .NotEmpty().WithMessage("Friendly URL is required.")
            .MustAsync(async (friendlyUrl, cancellation) =>
            {
                var exists = await context.Blogs.AnyAsync(b => b.FriendlyUrl == friendlyUrl, cancellation);
                return !exists;
            }).WithMessage("Friendly URL must be unique.");
    }
}