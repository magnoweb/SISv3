using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IProfessionalGroupCompetencyProfileService
{
    Task<IEnumerable<ProfessionalGroupCompetencyProfileDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<ProfessionalGroupCompetencyProfileDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ProfessionalGroupCompetencyProfileDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ProfessionalGroupCompetencyProfileDto> CreateAsync(CreateProfessionalGroupCompetencyProfileDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateProfessionalGroupCompetencyProfileDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
