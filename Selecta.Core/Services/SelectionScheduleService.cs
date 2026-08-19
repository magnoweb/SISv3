using Selecta.Core.Dtos;
using Selecta.Core.Entities.Schedule;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class SelectionScheduleService(ISelectionScheduleRepository repository) : ISelectionScheduleService
{
    public async Task<IEnumerable<SelectionScheduleDto>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await repository.GetAllAsync(ct);
        return items.Select(ToDto);
    }

    public async Task<PagedResult<SelectionScheduleDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<SelectionScheduleDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<SelectionScheduleDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var item = await repository.GetByIdAsync(id, ct);
        return item is null ? null : ToDto(item);
    }

    public async Task<SelectionScheduleDto> CreateAsync(CreateSelectionScheduleDto dto, CancellationToken ct = default)
    {
        // HasHistory calculado no servidor — mesmo critério de RecruitmentScheduleService.CreateAsync.
        var hasHistory = !string.IsNullOrWhiteSpace(dto.Cpf) && await repository.HasPriorEntriesAsync(dto.Cpf, ct);

        var item = new SelectionSchedule
        {
            JobTitleId = dto.JobTitleId,
            ContactId = dto.ContactId,
            Origin = dto.Origin,
            ClientNotes = dto.ClientNotes,
            Name = dto.Name,
            Cpf = dto.Cpf,
            Date = dto.Date,
            Time = dto.Time,
            InternalNotes = dto.InternalNotes,
            HasHistory = hasHistory,
            CreatedById = dto.CreatedById,
        };

        await repository.AddAsync(item, ct);
        await repository.SaveChangesAsync(ct);

        var created = await repository.GetByIdAsync(item.Id, ct) ?? item;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateSelectionScheduleDto dto, CancellationToken ct = default)
    {
        var item = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"SelectionSchedule {dto.Id} not found.");

        item.AssessmentEventId = dto.AssessmentEventId;
        item.JobTitleId = dto.JobTitleId;
        item.ContactId = dto.ContactId;
        item.Origin = dto.Origin;
        item.ClientNotes = dto.ClientNotes;
        item.Name = dto.Name;
        item.Cpf = dto.Cpf;
        item.Date = dto.Date;
        item.Time = dto.Time;
        item.Status = dto.Status;
        item.InternalNotes = dto.InternalNotes;

        repository.Update(item);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var item = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"SelectionSchedule {id} not found.");

        repository.Remove(item);
        await repository.SaveChangesAsync(ct);
    }

    private static SelectionScheduleDto ToDto(SelectionSchedule s) => new(
        s.Id, s.AssessmentEventId,
        s.JobTitleId, s.JobTitle?.Name ?? "(unknown)",
        s.ContactId, s.Contact?.Name,
        s.Origin, s.ClientNotes,
        s.Name, s.Cpf, s.Date, s.Time, s.Status,
        s.InternalNotes, s.HasHistory,
        s.CreatedById, s.CreatedBy?.Name ?? "(unknown)", s.CreatedAt);
}
