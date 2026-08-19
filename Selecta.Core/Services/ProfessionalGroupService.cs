using Selecta.Core.Dtos;
using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ProfessionalGroupService(IProfessionalGroupRepository repository) : IProfessionalGroupService
{
    public async Task<IEnumerable<ProfessionalGroupDto>> GetAllAsync(CancellationToken ct = default)
    {
        var groups = await repository.GetAllAsync(ct);
        return groups.Select(ToDto);
    }

    public async Task<PagedResult<ProfessionalGroupDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ProfessionalGroupDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ProfessionalGroupDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var group = await repository.GetByIdAsync(id, ct);
        return group is null ? null : ToDto(group);
    }

    public async Task<ProfessionalGroupDto> CreateAsync(CreateProfessionalGroupDto dto, CancellationToken ct = default)
    {
        var group = new ProfessionalGroup { Name = dto.Name, Description = dto.Description };
        await repository.AddAsync(group, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(group);
    }

    public async Task UpdateAsync(UpdateProfessionalGroupDto dto, CancellationToken ct = default)
    {
        var group = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"ProfessionalGroup {dto.Id} not found.");

        group.Name = dto.Name;
        group.Description = dto.Description;
        group.Active = dto.Active;

        repository.Update(group);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var group = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"ProfessionalGroup {id} not found.");

        repository.Remove(group);
        await repository.SaveChangesAsync(ct);
    }

    private static ProfessionalGroupDto ToDto(ProfessionalGroup g) => new(g.Id, g.Name, g.Description, g.Active, g.CreatedAt);
}
