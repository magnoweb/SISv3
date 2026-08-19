using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IAssessmentEventService
{
    Task<IEnumerable<AssessmentEventDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<AssessmentEventDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<AssessmentEventDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<AssessmentEventDto> CreateAsync(CreateAssessmentEventDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateAssessmentEventDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
