using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IScheduleNoteService
{
    Task<IEnumerable<ScheduleNoteDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<ScheduleNoteDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ScheduleNoteDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ScheduleNoteDto> CreateAsync(CreateScheduleNoteDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateScheduleNoteDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
