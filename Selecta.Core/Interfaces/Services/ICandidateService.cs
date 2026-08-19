using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface ICandidateService
{
    Task<IEnumerable<CandidateDto>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Página 1-based — usada pela tela de listagem.</summary>
    Task<PagedResult<CandidateDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);

    Task<CandidateDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Busca única por nome ou CPF (auto-deteta o termo), como na tela original de candidatos.</summary>
    Task<CandidateDto?> SearchByNameOrCpfAsync(string term, CancellationToken ct = default);

    /// <summary>Lança <see cref="Selecta.Core.Exceptions.DomainException"/> se o CPF for inválido ou já existir.</summary>
    Task<CandidateDto> CreateAsync(CreateCandidateDto dto, CancellationToken ct = default);

    /// <summary>Lança <see cref="Selecta.Core.Exceptions.DomainException"/> se o CPF for inválido ou já pertencer a outro candidato.</summary>
    Task UpdateAsync(UpdateCandidateDto dto, CancellationToken ct = default);

    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
