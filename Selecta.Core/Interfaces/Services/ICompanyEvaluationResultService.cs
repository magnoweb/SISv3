using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface ICompanyEvaluationResultService
{
    Task<IEnumerable<CompanyEvaluationResultDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<CompanyEvaluationResultDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<IEnumerable<CompanyEvaluationResultDto>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default);
    Task<CompanyEvaluationResultDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<CompanyEvaluationResultDto> CreateAsync(CreateCompanyEvaluationResultDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateCompanyEvaluationResultDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
