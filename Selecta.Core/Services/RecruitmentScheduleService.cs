using Selecta.Core.Dtos;
using Selecta.Core.Entities.Schedule;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class RecruitmentScheduleService(IRecruitmentScheduleRepository repository) : IRecruitmentScheduleService
{
    public async Task<IEnumerable<RecruitmentScheduleDto>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await repository.GetAllAsync(ct);
        return items.Select(ToDto);
    }

    public async Task<PagedResult<RecruitmentScheduleDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<RecruitmentScheduleDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<RecruitmentScheduleDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var item = await repository.GetByIdAsync(id, ct);
        return item is null ? null : ToDto(item);
    }

    public async Task<RecruitmentScheduleDto> CreateAsync(CreateRecruitmentScheduleDto dto, CancellationToken ct = default)
    {
        // HasHistory calculado no servidor — a v2 mostra o ícone de histórico automaticamente
        // quando já existem agendamentos anteriores para o mesmo CPF, não é algo que se marque à mão.
        var hasHistory = await repository.HasPriorEntriesAsync(dto.Cpf, ct);

        var item = new RecruitmentSchedule
        {
            JobOpeningId = dto.JobOpeningId,
            TicketId = dto.TicketId,
            ResponsibleId = dto.ResponsibleId,
            ClientInterview = dto.ClientInterview,
            ScheduleType = dto.ScheduleType,
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

    public async Task UpdateAsync(UpdateRecruitmentScheduleDto dto, CancellationToken ct = default)
    {
        var item = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"RecruitmentSchedule {dto.Id} not found.");

        item.JobOpeningId = dto.JobOpeningId;
        item.ResponsibleId = dto.ResponsibleId;
        item.ClientInterview = dto.ClientInterview;
        item.Hired = dto.Hired;
        item.ScheduleType = dto.ScheduleType;
        item.Result = dto.Result;
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
            ?? throw new KeyNotFoundException($"RecruitmentSchedule {id} not found.");

        repository.Remove(item);
        await repository.SaveChangesAsync(ct);
    }

    private static RecruitmentScheduleDto ToDto(RecruitmentSchedule s) => new(
        s.Id, s.JobOpeningId, s.JobOpening?.Name ?? "(unknown)", s.TicketId,
        s.ResponsibleId, s.Responsible?.Name ?? "(unknown)",
        s.ClientInterview, s.Hired, s.ScheduleType, s.Result,
        s.Name, s.Cpf, s.Date, s.Time, s.Status,
        s.InternalNotes, s.HasHistory,
        s.CreatedById, s.CreatedBy?.Name ?? "(unknown)", s.CreatedAt);
}
