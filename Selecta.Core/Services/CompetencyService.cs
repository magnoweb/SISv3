using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class CompetencyService(ICompetencyRepository repository) : ICompetencyService
{
    public async Task<IEnumerable<CompetencyDto>> GetAllAsync(CancellationToken ct = default)
    {
        var competencies = await repository.GetAllAsync(ct);
        return competencies.Select(ToDto);
    }

    public async Task<PagedResult<CompetencyDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<CompetencyDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<CompetencyDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var competency = await repository.GetByIdAsync(id, ct);
        return competency is null ? null : ToDto(competency);
    }

    public async Task<CompetencyDto> CreateAsync(CreateCompetencyDto dto, CancellationToken ct = default)
    {
        var competency = new Competency { Group = dto.Group, Name = dto.Name, Description = dto.Description };
        await repository.AddAsync(competency, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(competency);
    }

    public async Task UpdateAsync(UpdateCompetencyDto dto, CancellationToken ct = default)
    {
        var competency = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"Competency {dto.Id} not found.");

        competency.Group = dto.Group;
        competency.Name = dto.Name;
        competency.Description = dto.Description;
        competency.Active = dto.Active;

        repository.Update(competency);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var competency = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Competency {id} not found.");

        repository.Remove(competency);
        await repository.SaveChangesAsync(ct);
    }

    private static CompetencyDto ToDto(Competency c) => new(c.Id, c.Group, c.Name, c.Description, c.Active, c.CreatedAt);
}
