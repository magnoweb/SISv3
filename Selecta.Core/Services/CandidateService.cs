using Selecta.Core.Dtos;
using Selecta.Core.Entities.Common;
using Selecta.Core.Exceptions;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;
using Selecta.Core.Validation;

namespace Selecta.Core.Services;

public class CandidateService(ICandidateRepository repository) : ICandidateService
{
    public async Task<IEnumerable<CandidateDto>> GetAllAsync(CancellationToken ct = default)
    {
        var candidates = await repository.GetAllAsync(ct);
        return candidates.Select(ToDto);
    }

    public async Task<PagedResult<CandidateDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<CandidateDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<CandidateDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var candidate = await repository.GetByIdAsync(id, ct);
        return candidate is null ? null : ToDto(candidate);
    }

    public async Task<CandidateDto?> SearchByNameOrCpfAsync(string term, CancellationToken ct = default)
    {
        var candidate = await repository.SearchByNameOrCpfAsync(term, ct);
        return candidate is null ? null : ToDto(candidate);
    }

    public async Task<CandidateDto> CreateAsync(CreateCandidateDto dto, CancellationToken ct = default)
    {
        await ValidateCpfAsync(dto.Cpf, currentCandidateId: null, ct);

        var candidate = new Candidate
        {
            Name = dto.Name,
            Gender = dto.Gender,
            BirthDate = dto.BirthDate,
            Cpf = dto.Cpf,
            IdentityDocument = dto.IdentityDocument,
        };

        await repository.AddAsync(candidate, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(candidate);
    }

    public async Task UpdateAsync(UpdateCandidateDto dto, CancellationToken ct = default)
    {
        var candidate = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"Candidate {dto.Id} not found.");

        await ValidateCpfAsync(dto.Cpf, currentCandidateId: dto.Id, ct);

        candidate.Name = dto.Name;
        candidate.Gender = dto.Gender;
        candidate.BirthDate = dto.BirthDate;
        candidate.Cpf = dto.Cpf;
        candidate.IdentityDocument = dto.IdentityDocument;

        repository.Update(candidate);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var candidate = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Candidate {id} not found.");

        repository.Remove(candidate);
        await repository.SaveChangesAsync(ct);
    }

    private async Task ValidateCpfAsync(string cpf, Guid? currentCandidateId, CancellationToken ct)
    {
        if (!CpfValidator.IsValid(cpf))
            throw new DomainException("CPF inválido.");

        var existing = await repository.GetByCpfAsync(cpf, ct);
        if (existing is not null && existing.Id != currentCandidateId)
            throw new DomainException("Já existe um candidato com este CPF.");
    }

    private static CandidateDto ToDto(Candidate c) =>
        new(c.Id, c.Name, c.Gender, c.BirthDate, c.Cpf, c.IdentityDocument, c.CreatedAt);
}
