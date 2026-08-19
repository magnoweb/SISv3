using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface ISelectionScheduleService
{
    Task<IEnumerable<SelectionScheduleDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<SelectionScheduleDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<SelectionScheduleDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<SelectionScheduleDto> CreateAsync(CreateSelectionScheduleDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateSelectionScheduleDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
