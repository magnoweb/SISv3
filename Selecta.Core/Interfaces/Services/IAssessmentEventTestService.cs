using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IAssessmentEventTestService
{
    Task<IEnumerable<AssessmentEventTestDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<AssessmentEventTestDto>> GetByAssessmentEventAsync(Guid assessmentEventId, CancellationToken ct = default);
    Task<PagedResult<AssessmentEventTestDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<AssessmentEventTestDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<AssessmentEventTestDto> CreateAsync(CreateAssessmentEventTestDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateAssessmentEventTestDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
