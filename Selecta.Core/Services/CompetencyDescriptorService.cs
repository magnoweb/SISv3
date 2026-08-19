using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class CompetencyDescriptorService(ICompetencyDescriptorRepository repository) : ICompetencyDescriptorService
{
    public async Task<IEnumerable<CompetencyDescriptorDto>> GetAllAsync(CancellationToken ct = default)
    {
        var descriptors = await repository.GetAllAsync(ct);
        return descriptors.Select(ToDto);
    }

    public async Task<PagedResult<CompetencyDescriptorDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<CompetencyDescriptorDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<CompetencyDescriptorDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var descriptor = await repository.GetByIdAsync(id, ct);
        return descriptor is null ? null : ToDto(descriptor);
    }

    public async Task<CompetencyDescriptorDto> CreateAsync(CreateCompetencyDescriptorDto dto, CancellationToken ct = default)
    {
        var descriptor = new CompetencyDescriptor { CompetencyId = dto.CompetencyId, UserId = dto.UserId, Name = dto.Name };
        await repository.AddAsync(descriptor, ct);
        await repository.SaveChangesAsync(ct);

        // Recarrega com as relações incluídas, igual ao padrão de JobOpeningService.CreateAsync.
        var created = await repository.GetByIdAsync(descriptor.Id, ct) ?? descriptor;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateCompetencyDescriptorDto dto, CancellationToken ct = default)
    {
        var descriptor = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"CompetencyDescriptor {dto.Id} not found.");

        descriptor.CompetencyId = dto.CompetencyId;
        descriptor.UserId = dto.UserId;
        descriptor.Name = dto.Name;
        descriptor.Active = dto.Active;

        repository.Update(descriptor);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var descriptor = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"CompetencyDescriptor {id} not found.");

        repository.Remove(descriptor);
        await repository.SaveChangesAsync(ct);
    }

    private static CompetencyDescriptorDto ToDto(CompetencyDescriptor d) => new(
        d.Id, d.CompetencyId, d.Competency?.Name ?? "(unknown)", d.UserId, d.User?.Name ?? "(unknown)", d.Name, d.Active, d.CreatedAt);
}
