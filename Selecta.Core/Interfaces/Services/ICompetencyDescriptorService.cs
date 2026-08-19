using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface ICompetencyDescriptorService
{
    Task<IEnumerable<CompetencyDescriptorDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<CompetencyDescriptorDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<CompetencyDescriptorDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<CompetencyDescriptorDto> CreateAsync(CreateCompetencyDescriptorDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateCompetencyDescriptorDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
