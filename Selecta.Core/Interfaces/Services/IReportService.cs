using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IReportService
{
    Task<IEnumerable<ReportDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<ReportDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ReportDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Lança <see cref="Selecta.Core.Exceptions.DomainException"/> se o AssessmentEvent já tiver um Report.</summary>
    Task<ReportDto> CreateAsync(CreateReportDto dto, CancellationToken ct = default);

    Task UpdateAsync(UpdateReportDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
