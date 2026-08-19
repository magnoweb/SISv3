using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IEvaluationResultService
{
    Task<IEnumerable<EvaluationResultDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<EvaluationResultDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<EvaluationResultDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<EvaluationResultDto> CreateAsync(CreateEvaluationResultDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateEvaluationResultDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
