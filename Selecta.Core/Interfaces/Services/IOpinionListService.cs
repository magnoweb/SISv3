using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IOpinionListService
{
    Task<IEnumerable<OpinionListDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<OpinionListDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<OpinionListDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<OpinionListDto> CreateAsync(CreateOpinionListDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateOpinionListDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
