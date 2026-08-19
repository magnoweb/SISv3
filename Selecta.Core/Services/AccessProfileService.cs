using Selecta.Core.Dtos;
using Selecta.Core.Entities.Security;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class AccessProfileService(IAccessProfileRepository repository) : IAccessProfileService
{
    public async Task<IEnumerable<AccessProfileDto>> GetAllAsync(CancellationToken ct = default)
    {
        var profiles = await repository.GetAllAsync(ct);
        return profiles.Select(ToDto);
    }

    public async Task<PagedResult<AccessProfileDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<AccessProfileDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<AccessProfileDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var profile = await repository.GetByIdAsync(id, ct);
        return profile is null ? null : ToDto(profile);
    }

    public async Task<AccessProfileDto> CreateAsync(CreateAccessProfileDto dto, CancellationToken ct = default)
    {
        var profile = new AccessProfile { Name = dto.Name, Description = dto.Description };
        await repository.AddAsync(profile, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(profile);
    }

    public async Task UpdateAsync(UpdateAccessProfileDto dto, CancellationToken ct = default)
    {
        var profile = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"AccessProfile {dto.Id} not found.");

        profile.Name = dto.Name;
        profile.Description = dto.Description;

        repository.Update(profile);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var profile = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"AccessProfile {id} not found.");

        repository.Remove(profile);
        await repository.SaveChangesAsync(ct);
    }

    private static AccessProfileDto ToDto(AccessProfile p) => new(p.Id, p.Name, p.Description);
}
