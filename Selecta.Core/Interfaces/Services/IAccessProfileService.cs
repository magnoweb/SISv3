using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IAccessProfileService
{
    Task<IEnumerable<AccessProfileDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<AccessProfileDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<AccessProfileDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<AccessProfileDto> CreateAsync(CreateAccessProfileDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateAccessProfileDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
