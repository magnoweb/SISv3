using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IReportCompetencyService
{
    Task<IEnumerable<ReportCompetencyDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ReportCompetencyDto>> GetByReportAsync(Guid reportId, CancellationToken ct = default);
    Task<PagedResult<ReportCompetencyDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ReportCompetencyDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ReportCompetencyDto> CreateAsync(CreateReportCompetencyDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateReportCompetencyDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
