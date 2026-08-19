using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class CompetencyProfileLineService(ICompetencyProfileLineRepository repository) : ICompetencyProfileLineService
{
    public async Task<IEnumerable<CompetencyProfileLineDto>> GetAllAsync(CancellationToken ct = default)
    {
        var lines = await repository.GetAllAsync(ct);
        return lines.Select(ToDto);
    }

    public async Task<IEnumerable<CompetencyProfileLineDto>> GetByProfileAsync(Guid competencyProfileId, CancellationToken ct = default)
    {
        var lines = await repository.GetByProfileAsync(competencyProfileId, ct);
        return lines.Select(ToDto);
    }

    public async Task<PagedResult<CompetencyProfileLineDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<CompetencyProfileLineDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<CompetencyProfileLineDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var line = await repository.GetByIdAsync(id, ct);
        return line is null ? null : ToDto(line);
    }

    public async Task<CompetencyProfileLineDto> CreateAsync(CreateCompetencyProfileLineDto dto, CancellationToken ct = default)
    {
        var line = new CompetencyProfileLine { CompetencyProfileId = dto.CompetencyProfileId, CompetencyId = dto.CompetencyId, CompetencyScoreId = dto.CompetencyScoreId };
        await repository.AddAsync(line, ct);
        await repository.SaveChangesAsync(ct);

        var created = await repository.GetByIdAsync(line.Id, ct) ?? line;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateCompetencyProfileLineDto dto, CancellationToken ct = default)
    {
        var line = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"CompetencyProfileLine {dto.Id} not found.");

        line.CompetencyProfileId = dto.CompetencyProfileId;
        line.CompetencyId = dto.CompetencyId;
        line.CompetencyScoreId = dto.CompetencyScoreId;

        repository.Update(line);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var line = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"CompetencyProfileLine {id} not found.");

        repository.Remove(line);
        await repository.SaveChangesAsync(ct);
    }

    private static CompetencyProfileLineDto ToDto(CompetencyProfileLine l) => new(
        l.Id, l.CompetencyProfileId, l.CompetencyProfile?.Name ?? "(unknown)",
        l.CompetencyId, l.Competency?.Name ?? "(unknown)",
        l.CompetencyScoreId, l.CompetencyScore?.Name);
}
