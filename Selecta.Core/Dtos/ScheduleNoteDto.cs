using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record ScheduleNoteDto(Guid Id, ServiceOrigin Origin, DateTime Date, TimeSpan? Time, string Description, Guid CreatedById, string CreatedByName, DateTime CreatedAt);

public record CreateScheduleNoteDto(ServiceOrigin Origin, DateTime Date, TimeSpan? Time, string Description, Guid CreatedById);

public record UpdateScheduleNoteDto(Guid Id, ServiceOrigin Origin, DateTime Date, TimeSpan? Time, string Description);
