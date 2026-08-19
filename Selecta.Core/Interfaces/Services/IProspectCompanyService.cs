using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IProspectCompanyService
{
    Task<IEnumerable<ProspectCompanyDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<ProspectCompanyDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ProspectCompanyDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Lança <see cref="Selecta.Core.Exceptions.DomainException"/> se já existir um registo com o mesmo documento.</summary>
    Task<ProspectCompanyDto> CreateAsync(CreateProspectCompanyDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateProspectCompanyDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
