using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IOpinionListEntryService
{
    Task<IEnumerable<OpinionListEntryDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<OpinionListEntryDto>> GetByOpinionListAsync(Guid opinionListId, CancellationToken ct = default);
    Task<PagedResult<OpinionListEntryDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<OpinionListEntryDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<OpinionListEntryDto> CreateAsync(CreateOpinionListEntryDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateOpinionListEntryDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
