using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ProfessionalGroupCompetencyProfileService(IProfessionalGroupCompetencyProfileRepository repository) : IProfessionalGroupCompetencyProfileService
{
    public async Task<IEnumerable<ProfessionalGroupCompetencyProfileDto>> GetAllAsync(CancellationToken ct = default)
    {
        var profiles = await repository.GetAllAsync(ct);
        return profiles.Select(ToDto);
    }

    public async Task<PagedResult<ProfessionalGroupCompetencyProfileDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ProfessionalGroupCompetencyProfileDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ProfessionalGroupCompetencyProfileDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var profile = await repository.GetByIdAsync(id, ct);
        return profile is null ? null : ToDto(profile);
    }

    public async Task<ProfessionalGroupCompetencyProfileDto> CreateAsync(CreateProfessionalGroupCompetencyProfileDto dto, CancellationToken ct = default)
    {
        var profile = new ProfessionalGroupCompetencyProfile { Name = dto.Name, Description = dto.Description, ProfessionalGroupId = dto.ProfessionalGroupId };
        await repository.AddAsync(profile, ct);
        await repository.SaveChangesAsync(ct);

        var created = await repository.GetByIdAsync(profile.Id, ct) ?? profile;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateProfessionalGroupCompetencyProfileDto dto, CancellationToken ct = default)
    {
        var profile = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"ProfessionalGroupCompetencyProfile {dto.Id} not found.");

        profile.Name = dto.Name;
        profile.Description = dto.Description;
        profile.ProfessionalGroupId = dto.ProfessionalGroupId;
        profile.Active = dto.Active;

        repository.Update(profile);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var profile = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"ProfessionalGroupCompetencyProfile {id} not found.");

        repository.Remove(profile);
        await repository.SaveChangesAsync(ct);
    }

    private static ProfessionalGroupCompetencyProfileDto ToDto(ProfessionalGroupCompetencyProfile p) =>
        new(p.Id, p.Name, p.Description, p.ProfessionalGroupId, p.ProfessionalGroup?.Name, p.Active, p.CreatedAt);
}
