using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class OpinionListEntryService(IOpinionListEntryRepository repository) : IOpinionListEntryService
{
    public async Task<IEnumerable<OpinionListEntryDto>> GetAllAsync(CancellationToken ct = default)
    {
        var entries = await repository.GetAllAsync(ct);
        return entries.Select(ToDto);
    }

    public async Task<IEnumerable<OpinionListEntryDto>> GetByOpinionListAsync(Guid opinionListId, CancellationToken ct = default)
    {
        var entries = await repository.GetByOpinionListAsync(opinionListId, ct);
        return entries.Select(ToDto);
    }

    public async Task<PagedResult<OpinionListEntryDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<OpinionListEntryDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<OpinionListEntryDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entry = await repository.GetByIdAsync(id, ct);
        return entry is null ? null : ToDto(entry);
    }

    public async Task<OpinionListEntryDto> CreateAsync(CreateOpinionListEntryDto dto, CancellationToken ct = default)
    {
        var entry = new OpinionListEntry
        {
            OpinionListId = dto.OpinionListId,
            AssessmentEventId = dto.AssessmentEventId,
            Result = dto.Result,
            EvaluationResultId = dto.EvaluationResultId,
        };

        await repository.AddAsync(entry, ct);
        await repository.SaveChangesAsync(ct);

        var created = await repository.GetByIdAsync(entry.Id, ct) ?? entry;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateOpinionListEntryDto dto, CancellationToken ct = default)
    {
        var entry = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"OpinionListEntry {dto.Id} not found.");

        entry.OpinionListId = dto.OpinionListId;
        entry.AssessmentEventId = dto.AssessmentEventId;
        entry.Result = dto.Result;
        entry.EvaluationResultId = dto.EvaluationResultId;

        repository.Update(entry);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entry = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"OpinionListEntry {id} not found.");

        repository.Remove(entry);
        await repository.SaveChangesAsync(ct);
    }

    private static OpinionListEntryDto ToDto(OpinionListEntry e) => new(
        e.Id, e.OpinionListId,
        e.AssessmentEventId, e.AssessmentEvent?.Candidate?.Name ?? "(unknown)",
        e.Result,
        e.EvaluationResultId, e.EvaluationResult?.Name,
        e.CreatedAt);
}
