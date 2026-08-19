using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class JobTitleCompetencyProfileService(IJobTitleCompetencyProfileRepository repository) : IJobTitleCompetencyProfileService
{
    public async Task<IEnumerable<JobTitleCompetencyProfileDto>> GetAllAsync(CancellationToken ct = default)
    {
        var profiles = await repository.GetAllAsync(ct);
        return profiles.Select(ToDto);
    }

    public async Task<PagedResult<JobTitleCompetencyProfileDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<JobTitleCompetencyProfileDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<JobTitleCompetencyProfileDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var profile = await repository.GetByIdAsync(id, ct);
        return profile is null ? null : ToDto(profile);
    }

    public async Task<JobTitleCompetencyProfileDto> CreateAsync(CreateJobTitleCompetencyProfileDto dto, CancellationToken ct = default)
    {
        var profile = new JobTitleCompetencyProfile { Name = dto.Name, Description = dto.Description, JobTitleId = dto.JobTitleId };
        await repository.AddAsync(profile, ct);
        await repository.SaveChangesAsync(ct);

        var created = await repository.GetByIdAsync(profile.Id, ct) ?? profile;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateJobTitleCompetencyProfileDto dto, CancellationToken ct = default)
    {
        var profile = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"JobTitleCompetencyProfile {dto.Id} not found.");

        profile.Name = dto.Name;
        profile.Description = dto.Description;
        profile.JobTitleId = dto.JobTitleId;
        profile.Active = dto.Active;

        repository.Update(profile);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var profile = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"JobTitleCompetencyProfile {id} not found.");

        repository.Remove(profile);
        await repository.SaveChangesAsync(ct);
    }

    private static JobTitleCompetencyProfileDto ToDto(JobTitleCompetencyProfile p) =>
        new(p.Id, p.Name, p.Description, p.JobTitleId, p.JobTitle?.Name, p.Active, p.CreatedAt);
}
