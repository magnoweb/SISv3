using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IJobTitleCompetencyProfileService
{
    Task<IEnumerable<JobTitleCompetencyProfileDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<JobTitleCompetencyProfileDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<JobTitleCompetencyProfileDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<JobTitleCompetencyProfileDto> CreateAsync(CreateJobTitleCompetencyProfileDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateJobTitleCompetencyProfileDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
