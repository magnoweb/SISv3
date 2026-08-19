namespace Selecta.Core.Dtos;

public record CollaboratorDto(Guid Id, string Name, string? Document, bool Active, DateTime CreatedAt);

public record CreateCollaboratorDto(string Name, string? Document);

public record UpdateCollaboratorDto(Guid Id, string Name, string? Document, bool Active);
