using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IReportTemplateComponentService
{
    Task<IEnumerable<ReportTemplateComponentDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ReportTemplateComponentDto>> GetByReportTemplateAsync(int reportTemplateId, CancellationToken ct = default);
    Task<PagedResult<ReportTemplateComponentDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ReportTemplateComponentDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ReportTemplateComponentDto> CreateAsync(CreateReportTemplateComponentDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateReportTemplateComponentDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
