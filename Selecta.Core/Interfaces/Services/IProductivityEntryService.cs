using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IProductivityEntryService
{
    Task<IEnumerable<ProductivityEntryDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ProductivityEntryDto>> GetByAssessmentEventAsync(Guid assessmentEventId, CancellationToken ct = default);
    Task<PagedResult<ProductivityEntryDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ProductivityEntryDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ProductivityEntryDto> CreateAsync(CreateProductivityEntryDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateProductivityEntryDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
