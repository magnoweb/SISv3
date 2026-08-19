using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IProposalService
{
    Task<IEnumerable<ProposalDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<ProposalDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<ProposalDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ProposalDto> CreateAsync(CreateProposalDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateProposalDto dto, CancellationToken ct = default);

    /// <summary>Lança <see cref="Selecta.Core.Exceptions.DomainException"/> se faltar o motivo de recusa ao recusar.</summary>
    Task ChangeStatusAsync(ChangeProposalStatusDto dto, CancellationToken ct = default);

    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
