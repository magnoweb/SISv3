using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IReportTemplateService
{
    Task<IEnumerable<ReportTemplateDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<ReportTemplateDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ReportTemplateDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ReportTemplateDto> CreateAsync(CreateReportTemplateDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateReportTemplateDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
