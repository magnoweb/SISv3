namespace Selecta.Core.Dtos;

public record PsychologicalTestDto(Guid Id, string Name, string? Description, bool Active, DateTime CreatedAt);

public record CreatePsychologicalTestDto(string Name, string? Description);

public record UpdatePsychologicalTestDto(Guid Id, string Name, string? Description, bool Active);
