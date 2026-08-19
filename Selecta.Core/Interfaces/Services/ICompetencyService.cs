using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface ICompetencyService
{
    Task<IEnumerable<CompetencyDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<CompetencyDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<CompetencyDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<CompetencyDto> CreateAsync(CreateCompetencyDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateCompetencyDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
