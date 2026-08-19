using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IScheduleBlockService
{
    Task<IEnumerable<ScheduleBlockDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<ScheduleBlockDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ScheduleBlockDto> CreateAsync(CreateScheduleBlockDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
