using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IJobTitleService
{
    Task<IEnumerable<JobTitleDto>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Página 1-based — usada pela tela de listagem.</summary>
    Task<PagedResult<JobTitleDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);

    Task<IEnumerable<JobTitleDto>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default);
    Task<JobTitleDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<JobTitleDto> CreateAsync(CreateJobTitleDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateJobTitleDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
