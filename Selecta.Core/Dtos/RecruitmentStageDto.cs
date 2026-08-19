namespace Selecta.Core.Dtos;

public record RecruitmentStageDto(Guid Id, string Name, string? Description, string? CssClass, int Order, bool Active, DateTime CreatedAt);

public record CreateRecruitmentStageDto(string Name, string? Description, string? CssClass, int Order);

public record UpdateRecruitmentStageDto(Guid Id, string Name, string? Description, string? CssClass, int Order, bool Active);
