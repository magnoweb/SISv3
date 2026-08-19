using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IProfessionalGroupService
{
    Task<IEnumerable<ProfessionalGroupDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<ProfessionalGroupDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ProfessionalGroupDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ProfessionalGroupDto> CreateAsync(CreateProfessionalGroupDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateProfessionalGroupDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
