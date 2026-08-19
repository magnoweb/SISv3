using Selecta.Core.Dtos;
using Selecta.Core.Entities.Recruitment;
using Selecta.Core.Exceptions;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;
using Selecta.Core.Validation;

namespace Selecta.Core.Services;

public class JobOpeningService(IJobOpeningRepository repository) : IJobOpeningService
{
    public async Task<IEnumerable<JobOpeningDto>> GetAllAsync(CancellationToken ct = default)
    {
        var jobs = await repository.GetAllAsync(ct);
        return jobs.Select(ToDto);
    }

    public async Task<PagedResult<JobOpeningDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<JobOpeningDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<PagedResult<JobOpeningDto>> GetPagedAsync(int page, int pageSize, bool activeOnly, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, activeOnly, filter, orderBy, ct);
        return new PagedResult<JobOpeningDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<IEnumerable<JobOpeningDto>> GetActiveAsync(CancellationToken ct = default)
    {
        var jobs = await repository.GetActiveAsync(ct);
        return jobs.Select(ToDto);
    }

    public async Task<JobOpeningDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var job = await repository.GetByIdAsync(id, ct);
        return job is null ? null : ToDto(job);
    }

    public async Task<JobOpeningDto> CreateAsync(CreateJobOpeningDto dto, CancellationToken ct = default)
    {
        var job = new JobOpening
        {
            TicketId = dto.TicketId,
            ManagerId = dto.ManagerId,
            ContactId = dto.ContactId,
            JobTitleId = dto.JobTitleId,
            RecruitmentStageId = dto.RecruitmentStageId,
            Name = dto.Name,
            Summary = dto.Summary,
            DeadlineDays = dto.DeadlineDays,
            Quantity = dto.Quantity,
            Salary = dto.Salary,
            CreatedById = dto.CreatedById,
        };

        await repository.AddAsync(job, ct);
        await repository.SaveChangesAsync(ct);

        // Recarrega com as relações incluídas (Manager/Contact/JobTitle/RecruitmentStage),
        // já que a instância recém-criada não as tem carregadas em memória.
        var created = await repository.GetByIdAsync(job.Id, ct) ?? job;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateJobOpeningDto dto, CancellationToken ct = default)
    {
        var job = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"JobOpening {dto.Id} not found.");

        job.ManagerId = dto.ManagerId;
        job.ContactId = dto.ContactId;
        job.JobTitleId = dto.JobTitleId;
        job.RecruitmentStageId = dto.RecruitmentStageId;
        job.Name = dto.Name;
        job.Summary = dto.Summary;
        job.DeadlineDays = dto.DeadlineDays;
        job.Quantity = dto.Quantity;
        job.Salary = dto.Salary;
        job.PaymentDate = dto.PaymentDate;

        repository.Update(job);
        await repository.SaveChangesAsync(ct);
    }

    public async Task ChangeStatusAsync(ChangeJobOpeningStatusDto dto, CancellationToken ct = default)
    {
        var job = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"JobOpening {dto.Id} not found.");

        if (!JobOpeningStatusRules.CanTransition(job.Status, dto.NewStatus))
            throw new DomainException("Não é permitido mudar para o status selecionado.");

        job.Status = dto.NewStatus;

        // Ao finalizar, regista o fecho — igual ao comportamento original (DataFechamento).
        if (dto.NewStatus == Entities.Enums.JobOpeningStatus.Finished)
            job.ClosedAt = DateTime.UtcNow;

        repository.Update(job);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var job = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"JobOpening {id} not found.");

        repository.Remove(job);
        await repository.SaveChangesAsync(ct);
    }

    private static JobOpeningDto ToDto(JobOpening j) => new(
        j.Id, j.TicketId,
        j.ManagerId, j.Manager?.Name ?? "(unknown)",
        j.ContactId, j.Contact?.Name ?? "(unknown)",
        j.JobTitleId, j.JobTitle?.Name ?? "(unknown)",
        j.RecruitmentStageId, j.RecruitmentStage?.Name ?? "(unknown)",
        j.Name, j.Summary, j.DeadlineDays, j.Quantity, j.Salary, j.Status, j.CreatedById,
        j.PaymentDate, j.ClosedAt, j.CreatedAt,
        CalculateOpenDuration(j), CalculateWorkingDaysOpen(j));

    /// <summary>
    /// Porta de Vaga.TempoVaga. O original convertia a hora local usando o fuso
    /// "E. South America Standard Time" (um id de fuso só do Windows — quebraria
    /// em runtime Linux). Aqui trabalha-se inteiramente em UTC, mais portável;
    /// ajusta para o fuso correto na apresentação (Web), se precisares da hora
    /// local exata do Brasil.
    /// </summary>
    private static string CalculateOpenDuration(JobOpening j)
    {
        var end = j.ClosedAt ?? DateTime.UtcNow;
        var elapsed = end - j.CreatedAt;

        if (elapsed.Days > 0) return $"{elapsed.Days} days";
        if (elapsed.Hours >= 1) return $"{elapsed.Hours} hours";
        return "Less than 1 hour";
    }

    /// <summary>Porta de Vaga.DiasTrabalhados (dias úteis, exclui sábado/domingo).</summary>
    private static int CalculateWorkingDaysOpen(JobOpening j)
    {
        var end = j.ClosedAt ?? DateTime.UtcNow;
        var totalDays = Math.Max(0, (int)(end - j.CreatedAt).TotalDays);

        return Enumerable.Range(1, totalDays)
            .Select(offset => j.CreatedAt.AddDays(offset))
            .Count(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday);
    }
}
