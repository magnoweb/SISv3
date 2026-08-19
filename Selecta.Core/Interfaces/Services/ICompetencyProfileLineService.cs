using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface ICompetencyProfileLineService
{
    Task<IEnumerable<CompetencyProfileLineDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<CompetencyProfileLineDto>> GetByProfileAsync(Guid competencyProfileId, CancellationToken ct = default);
    Task<PagedResult<CompetencyProfileLineDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<CompetencyProfileLineDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<CompetencyProfileLineDto> CreateAsync(CreateCompetencyProfileLineDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateCompetencyProfileLineDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
