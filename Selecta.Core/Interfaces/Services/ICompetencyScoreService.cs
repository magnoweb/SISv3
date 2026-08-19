using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface ICompetencyScoreService
{
    Task<IEnumerable<CompetencyScoreDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<CompetencyScoreDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<CompetencyScoreDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<CompetencyScoreDto> CreateAsync(CreateCompetencyScoreDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateCompetencyScoreDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
