using BlogAPI.Domain.Entity;

namespace BlogAPI.Domain.Model;

public record BlogsPagedDto(List<BlogDto> Blogs, int Page, int PageSize, int Total);