using Selecta.Core.Dtos;
using Selecta.Core.Entities.Schedule;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ScheduleNoteService(IScheduleNoteRepository repository) : IScheduleNoteService
{
    public async Task<IEnumerable<ScheduleNoteDto>> GetAllAsync(CancellationToken ct = default)
    {
        var notes = await repository.GetAllAsync(ct);
        return notes.Select(ToDto);
    }

    public async Task<PagedResult<ScheduleNoteDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ScheduleNoteDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ScheduleNoteDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var note = await repository.GetByIdAsync(id, ct);
        return note is null ? null : ToDto(note);
    }

    public async Task<ScheduleNoteDto> CreateAsync(CreateScheduleNoteDto dto, CancellationToken ct = default)
    {
        var note = new ScheduleNote
        {
            Origin = dto.Origin,
            Date = dto.Date,
            Time = dto.Time,
            Description = dto.Description,
            CreatedById = dto.CreatedById,
        };

        await repository.AddAsync(note, ct);
        await repository.SaveChangesAsync(ct);

        var created = await repository.GetByIdAsync(note.Id, ct) ?? note;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateScheduleNoteDto dto, CancellationToken ct = default)
    {
        var note = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"ScheduleNote {dto.Id} not found.");

        note.Origin = dto.Origin;
        note.Date = dto.Date;
        note.Time = dto.Time;
        note.Description = dto.Description;

        repository.Update(note);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var note = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"ScheduleNote {id} not found.");

        repository.Remove(note);
        await repository.SaveChangesAsync(ct);
    }

    private static ScheduleNoteDto ToDto(ScheduleNote n) => new(
        n.Id, n.Origin, n.Date, n.Time, n.Description,
        n.CreatedById, n.CreatedBy?.Name ?? "(unknown)", n.CreatedAt);
}
