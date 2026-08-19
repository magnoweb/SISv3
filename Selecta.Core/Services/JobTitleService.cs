using Selecta.Core.Dtos;
using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class JobTitleService(IJobTitleRepository repository) : IJobTitleService
{
    public async Task<IEnumerable<JobTitleDto>> GetAllAsync(CancellationToken ct = default)
    {
        var jobTitles = await repository.GetAllAsync(ct);
        return jobTitles.Select(ToDto);
    }

    public async Task<PagedResult<JobTitleDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<JobTitleDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<IEnumerable<JobTitleDto>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default)
    {
        var jobTitles = await repository.GetByCompanyAsync(companyId, ct);
        return jobTitles.Select(ToDto);
    }

    public async Task<JobTitleDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var jobTitle = await repository.GetByIdAsync(id, ct);
        return jobTitle is null ? null : ToDto(jobTitle);
    }

    public async Task<JobTitleDto> CreateAsync(CreateJobTitleDto dto, CancellationToken ct = default)
    {
        var jobTitle = new JobTitle
        {
            CompanyId = dto.CompanyId,
            ProfessionalGroupId = dto.ProfessionalGroupId,
            Name = dto.Name,
            Description = dto.Description,
        };

        await repository.AddAsync(jobTitle, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(jobTitle);
    }

    public async Task UpdateAsync(UpdateJobTitleDto dto, CancellationToken ct = default)
    {
        var jobTitle = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"JobTitle {dto.Id} not found.");

        jobTitle.CompanyId = dto.CompanyId;
        jobTitle.ProfessionalGroupId = dto.ProfessionalGroupId;
        jobTitle.Name = dto.Name;
        jobTitle.Description = dto.Description;
        jobTitle.Active = dto.Active;

        repository.Update(jobTitle);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var jobTitle = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"JobTitle {id} not found.");

        repository.Remove(jobTitle);
        await repository.SaveChangesAsync(ct);
    }

    private static JobTitleDto ToDto(JobTitle j) =>
        new(j.Id, j.CompanyId, j.ProfessionalGroupId, j.Name, j.Description, j.Active, j.CreatedAt);
}
