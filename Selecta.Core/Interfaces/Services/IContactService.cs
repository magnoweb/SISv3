using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IContactService
{
    Task<IEnumerable<ContactDto>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Página 1-based — usada pela tela de listagem.</summary>
    Task<PagedResult<ContactDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);

    Task<IEnumerable<ContactDto>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default);
    Task<ContactDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ContactDto> CreateAsync(CreateContactDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateContactDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
