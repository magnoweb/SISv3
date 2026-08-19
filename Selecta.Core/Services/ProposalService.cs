using Selecta.Core.Dtos;
using Selecta.Core.Entities.Administrative;
using Selecta.Core.Entities.Enums;
using Selecta.Core.Exceptions;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ProposalService(IProposalRepository repository) : IProposalService
{
    public async Task<IEnumerable<ProposalDto>> GetAllAsync(CancellationToken ct = default)
    {
        var proposals = await repository.GetAllAsync(ct);
        return proposals.Select(ToDto);
    }

    public async Task<PagedResult<ProposalDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ProposalDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ProposalDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var proposal = await repository.GetByIdAsync(id, ct);
        return proposal is null ? null : ToDto(proposal);
    }

    public async Task<ProposalDto> CreateAsync(CreateProposalDto dto, CancellationToken ct = default)
    {
        var proposal = new Proposal
        {
            ServiceOfferingId = dto.ServiceOfferingId,
            ProspectCompanyId = dto.ProspectCompanyId,
            CreatedById = dto.CreatedById,
            Name = dto.Name,
            Description = dto.Description,
        };

        await repository.AddAsync(proposal, ct);
        await repository.SaveChangesAsync(ct);

        // Recarrega com as relações incluídas, igual ao que fazemos em JobOpeningService.CreateAsync.
        var created = await repository.GetByIdAsync(proposal.Id, ct) ?? proposal;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateProposalDto dto, CancellationToken ct = default)
    {
        var proposal = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"Proposal {dto.Id} not found.");

        proposal.ServiceOfferingId = dto.ServiceOfferingId;
        proposal.ProspectCompanyId = dto.ProspectCompanyId;
        proposal.Name = dto.Name;
        proposal.Description = dto.Description;
        proposal.UpdatedAt = DateTime.UtcNow;

        repository.Update(proposal);
        await repository.SaveChangesAsync(ct);
    }

    public async Task ChangeStatusAsync(ChangeProposalStatusDto dto, CancellationToken ct = default)
    {
        var proposal = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"Proposal {dto.Id} not found.");

        if (dto.NewStatus == ProposalStatus.Declined && dto.DeclineReason is null)
            throw new DomainException("Informe o motivo da recusa.");

        proposal.Status = dto.NewStatus;
        proposal.DeclineReason = dto.NewStatus == ProposalStatus.Declined ? dto.DeclineReason : null;
        proposal.UpdatedAt = DateTime.UtcNow;

        repository.Update(proposal);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var proposal = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Proposal {id} not found.");

        repository.Remove(proposal);
        await repository.SaveChangesAsync(ct);
    }

    private static ProposalDto ToDto(Proposal p)
    {
        var (totalDays, totalWorkingDays) = CalculateDays(p);

        return new ProposalDto(
            p.Id, p.ServiceOfferingId, p.ServiceOffering?.Name ?? "(unknown)",
            p.ProspectCompanyId, p.ProspectCompany?.Name ?? "(unknown)",
            p.CreatedById, p.Name, p.Description, p.Status, p.DeclineReason,
            p.CreatedAt, p.UpdatedAt, totalDays, totalWorkingDays);
    }

    /// <summary>Porta de Proposta.Dias — em UTC, pelo mesmo motivo de portabilidade explicado em JobOpeningService.</summary>
    private static (int TotalDays, int TotalWorkingDays) CalculateDays(Proposal p)
    {
        var end = p.UpdatedAt ?? DateTime.UtcNow;
        var totalDays = Math.Max(0, (int)(end - p.CreatedAt).TotalDays);

        var totalWorkingDays = Enumerable.Range(1, totalDays)
            .Select(offset => p.CreatedAt.AddDays(offset))
            .Count(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday);

        return (totalDays, totalWorkingDays);
    }
}
