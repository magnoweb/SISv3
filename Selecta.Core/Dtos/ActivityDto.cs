using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record ActivityDto(int Id, string Name, int Duration, bool FlexibleDuration, ServiceOrigin Origin, bool System, bool Active);

public record CreateActivityDto(string Name, int Duration, bool FlexibleDuration, ServiceOrigin Origin);

public record UpdateActivityDto(int Id, string Name, int Duration, bool FlexibleDuration, ServiceOrigin Origin, bool Active);
