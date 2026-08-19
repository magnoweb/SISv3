using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ProductivityEntryService(IProductivityEntryRepository repository) : IProductivityEntryService
{
    public async Task<IEnumerable<ProductivityEntryDto>> GetAllAsync(CancellationToken ct = default)
    {
        var entries = await repository.GetAllAsync(ct);
        return entries.Select(ToDto);
    }

    public async Task<IEnumerable<ProductivityEntryDto>> GetByAssessmentEventAsync(Guid assessmentEventId, CancellationToken ct = default)
    {
        var entries = await repository.GetByAssessmentEventAsync(assessmentEventId, ct);
        return entries.Select(ToDto);
    }

    public async Task<PagedResult<ProductivityEntryDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ProductivityEntryDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ProductivityEntryDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entry = await repository.GetByIdAsync(id, ct);
        return entry is null ? null : ToDto(entry);
    }

    public async Task<ProductivityEntryDto> CreateAsync(CreateProductivityEntryDto dto, CancellationToken ct = default)
    {
        var entry = new ProductivityEntry
        {
            AssessmentEventId = dto.AssessmentEventId,
            ActivityId = dto.ActivityId,
            Date = dto.Date,
            Duration = dto.Duration,
            UserId = dto.UserId,
        };

        await repository.AddAsync(entry, ct);
        await repository.SaveChangesAsync(ct);

        var created = await repository.GetByIdAsync(entry.Id, ct) ?? entry;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateProductivityEntryDto dto, CancellationToken ct = default)
    {
        var entry = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"ProductivityEntry {dto.Id} not found.");

        entry.AssessmentEventId = dto.AssessmentEventId;
        entry.ActivityId = dto.ActivityId;
        entry.Date = dto.Date;
        entry.Duration = dto.Duration;
        entry.UserId = dto.UserId;

        repository.Update(entry);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entry = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"ProductivityEntry {id} not found.");

        repository.Remove(entry);
        await repository.SaveChangesAsync(ct);
    }

    private static ProductivityEntryDto ToDto(ProductivityEntry e) => new(
        e.Id, e.AssessmentEventId, e.AssessmentEvent?.Candidate?.Name ?? "(unknown)",
        e.ActivityId, e.Activity?.Name ?? "(unknown)",
        e.Date, e.Duration,
        e.UserId, e.User?.Name ?? "(unknown)",
        e.CreatedAt);
}
