using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IServiceOfferingService
{
    Task<IEnumerable<ServiceOfferingDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<ServiceOfferingDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ServiceOfferingDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ServiceOfferingDto> CreateAsync(CreateServiceOfferingDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateServiceOfferingDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
