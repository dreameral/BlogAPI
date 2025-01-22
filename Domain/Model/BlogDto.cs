using MediatR;

namespace BlogAPI.Domain.Model;

public record BlogDto(
    string? Id,
    string Title,
    string FriendlyUrl,
    string Content,
    DateTime DateCreated);