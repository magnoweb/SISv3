using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class CompetencyScoreDescriptorService(ICompetencyScoreDescriptorRepository repository) : ICompetencyScoreDescriptorService
{
    public async Task<IEnumerable<CompetencyScoreDescriptorDto>> GetAllAsync(CancellationToken ct = default)
    {
        var descriptors = await repository.GetAllAsync(ct);
        return descriptors.Select(ToDto);
    }

    public async Task<PagedResult<CompetencyScoreDescriptorDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<CompetencyScoreDescriptorDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<CompetencyScoreDescriptorDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var descriptor = await repository.GetByIdAsync(id, ct);
        return descriptor is null ? null : ToDto(descriptor);
    }

    public async Task<CompetencyScoreDescriptorDto> CreateAsync(CreateCompetencyScoreDescriptorDto dto, CancellationToken ct = default)
    {
        var descriptor = new CompetencyScoreDescriptor
        {
            CompetencyDescriptorId = dto.CompetencyDescriptorId,
            CompetencyScoreId = dto.CompetencyScoreId,
            Descriptive = dto.Descriptive,
        };

        await repository.AddAsync(descriptor, ct);
        await repository.SaveChangesAsync(ct);

        // Recarrega com as relações incluídas, igual ao padrão de JobOpeningService.CreateAsync.
        var created = await repository.GetByIdAsync(descriptor.Id, ct) ?? descriptor;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateCompetencyScoreDescriptorDto dto, CancellationToken ct = default)
    {
        var descriptor = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"CompetencyScoreDescriptor {dto.Id} not found.");

        descriptor.CompetencyDescriptorId = dto.CompetencyDescriptorId;
        descriptor.CompetencyScoreId = dto.CompetencyScoreId;
        descriptor.Descriptive = dto.Descriptive;
        descriptor.Active = dto.Active;

        repository.Update(descriptor);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var descriptor = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"CompetencyScoreDescriptor {id} not found.");

        repository.Remove(descriptor);
        await repository.SaveChangesAsync(ct);
    }

    private static CompetencyScoreDescriptorDto ToDto(CompetencyScoreDescriptor d) => new(
        d.Id, d.CompetencyDescriptorId, d.CompetencyDescriptor?.Name ?? "(unknown)",
        d.CompetencyScoreId, d.CompetencyScore?.Name ?? "(unknown)", d.Descriptive, d.Active, d.CreatedAt);
}
