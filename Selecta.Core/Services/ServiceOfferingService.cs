using Selecta.Core.Dtos;
using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ServiceOfferingService(IServiceOfferingRepository repository) : IServiceOfferingService
{
    public async Task<IEnumerable<ServiceOfferingDto>> GetAllAsync(CancellationToken ct = default)
    {
        var offerings = await repository.GetAllAsync(ct);
        return offerings.Select(ToDto);
    }

    public async Task<PagedResult<ServiceOfferingDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ServiceOfferingDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ServiceOfferingDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var offering = await repository.GetByIdAsync(id, ct);
        return offering is null ? null : ToDto(offering);
    }

    public async Task<ServiceOfferingDto> CreateAsync(CreateServiceOfferingDto dto, CancellationToken ct = default)
    {
        var offering = new ServiceOffering
        {
            Name = dto.Name,
            Description = dto.Description,
            Recruitment = dto.Recruitment,
            Selection = dto.Selection,
            Proposal = dto.Proposal,
        };

        await repository.AddAsync(offering, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(offering);
    }

    public async Task UpdateAsync(UpdateServiceOfferingDto dto, CancellationToken ct = default)
    {
        var offering = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"ServiceOffering {dto.Id} not found.");

        offering.Name = dto.Name;
        offering.Description = dto.Description;
        offering.Recruitment = dto.Recruitment;
        offering.Selection = dto.Selection;
        offering.Proposal = dto.Proposal;
        offering.Active = dto.Active;

        repository.Update(offering);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var offering = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"ServiceOffering {id} not found.");

        repository.Remove(offering);
        await repository.SaveChangesAsync(ct);
    }

    private static ServiceOfferingDto ToDto(ServiceOffering s) =>
        new(s.Id, s.Name, s.Description, s.Recruitment, s.Selection, s.Proposal, s.Active);
}
