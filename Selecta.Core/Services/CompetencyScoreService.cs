using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class CompetencyScoreService(ICompetencyScoreRepository repository) : ICompetencyScoreService
{
    public async Task<IEnumerable<CompetencyScoreDto>> GetAllAsync(CancellationToken ct = default)
    {
        var scores = await repository.GetAllAsync(ct);
        return scores.Select(ToDto);
    }

    public async Task<PagedResult<CompetencyScoreDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<CompetencyScoreDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<CompetencyScoreDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var score = await repository.GetByIdAsync(id, ct);
        return score is null ? null : ToDto(score);
    }

    public async Task<CompetencyScoreDto> CreateAsync(CreateCompetencyScoreDto dto, CancellationToken ct = default)
    {
        var score = new CompetencyScore { Name = dto.Name, Acronym = dto.Acronym, Color = dto.Color, Value = dto.Value, Description = dto.Description };
        await repository.AddAsync(score, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(score);
    }

    public async Task UpdateAsync(UpdateCompetencyScoreDto dto, CancellationToken ct = default)
    {
        var score = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"CompetencyScore {dto.Id} not found.");

        score.Name = dto.Name;
        score.Acronym = dto.Acronym;
        score.Color = dto.Color;
        score.Value = dto.Value;
        score.Description = dto.Description;
        score.Active = dto.Active;

        repository.Update(score);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var score = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"CompetencyScore {id} not found.");

        repository.Remove(score);
        await repository.SaveChangesAsync(ct);
    }

    private static CompetencyScoreDto ToDto(CompetencyScore s) => new(s.Id, s.Name, s.Acronym, s.Color, s.Value, s.Description, s.Active);
}
