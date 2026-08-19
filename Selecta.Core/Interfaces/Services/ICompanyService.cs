using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Página 1-based — usada pela tela de listagem.</summary>
    Task<PagedResult<CompanyDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);

    Task<CompanyDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Lança <see cref="Selecta.Core.Exceptions.DomainException"/> se já existir uma empresa com o mesmo documento.</summary>
    Task<CompanyDto> CreateAsync(CreateCompanyDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateCompanyDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
