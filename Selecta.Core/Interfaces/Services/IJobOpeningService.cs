using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IJobOpeningService
{
    Task<IEnumerable<JobOpeningDto>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Página 1-based, mais recentes primeiro — usada pela tela de listagem.</summary>
    Task<PagedResult<JobOpeningDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);

    /// <summary>Sobrecarga com filtro "apenas ativas" — mantém o TotalCount correto quando o filtro está ligado.</summary>
    Task<PagedResult<JobOpeningDto>> GetPagedAsync(int page, int pageSize, bool activeOnly, string? filter = null, string? orderBy = null, CancellationToken ct = default);

    Task<IEnumerable<JobOpeningDto>> GetActiveAsync(CancellationToken ct = default);
    Task<JobOpeningDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<JobOpeningDto> CreateAsync(CreateJobOpeningDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateJobOpeningDto dto, CancellationToken ct = default);

    /// <summary>Lança <see cref="Selecta.Core.Exceptions.DomainException"/> se a transição de status não for permitida.</summary>
    Task ChangeStatusAsync(ChangeJobOpeningStatusDto dto, CancellationToken ct = default);

    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
