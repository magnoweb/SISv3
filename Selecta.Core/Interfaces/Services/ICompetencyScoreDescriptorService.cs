using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface ICompetencyScoreDescriptorService
{
    Task<IEnumerable<CompetencyScoreDescriptorDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<CompetencyScoreDescriptorDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<CompetencyScoreDescriptorDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<CompetencyScoreDescriptorDto> CreateAsync(CreateCompetencyScoreDescriptorDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateCompetencyScoreDescriptorDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
