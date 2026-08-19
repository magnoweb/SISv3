using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IRecruitmentScheduleService
{
    Task<IEnumerable<RecruitmentScheduleDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<RecruitmentScheduleDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<RecruitmentScheduleDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<RecruitmentScheduleDto> CreateAsync(CreateRecruitmentScheduleDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateRecruitmentScheduleDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
