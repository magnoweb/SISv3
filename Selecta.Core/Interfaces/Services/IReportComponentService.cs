using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IReportComponentService
{
    Task<IEnumerable<ReportComponentDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<ReportComponentDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ReportComponentDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ReportComponentDto> CreateAsync(CreateReportComponentDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateReportComponentDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
