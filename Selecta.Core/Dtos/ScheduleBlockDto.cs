using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record ScheduleBlockDto(Guid Id, ServiceOrigin Origin, DateTime Date, TimeSpan? Time, Guid UserId, string UserName, DateTime CreatedAt);

public record CreateScheduleBlockDto(ServiceOrigin Origin, DateTime Date, TimeSpan? Time, Guid UserId);
