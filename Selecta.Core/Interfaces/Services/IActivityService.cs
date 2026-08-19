using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IActivityService
{
    Task<IEnumerable<ActivityDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<ActivityDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ActivityDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ActivityDto> CreateAsync(CreateActivityDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateActivityDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
