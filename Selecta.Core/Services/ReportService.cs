using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Exceptions;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ReportService(IReportRepository repository) : IReportService
{
    public async Task<IEnumerable<ReportDto>> GetAllAsync(CancellationToken ct = default)
    {
        var reports = await repository.GetAllAsync(ct);
        return reports.Select(ToDto);
    }

    public async Task<PagedResult<ReportDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ReportDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ReportDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var report = await repository.GetByIdAsync(id, ct);
        return report is null ? null : ToDto(report);
    }

    public async Task<ReportDto> CreateAsync(CreateReportDto dto, CancellationToken ct = default)
    {
        var existing = await repository.GetByAssessmentEventAsync(dto.AssessmentEventId, ct);
        if (existing is not null)
            throw new DomainException("Este evento de avaliação já tem um laudo.");

        var report = new Report
        {
            AssessmentEventId = dto.AssessmentEventId,
            ReportTemplateId = dto.ReportTemplateId,
            Descriptive = dto.Descriptive,
            ResponsibleId = dto.ResponsibleId,
            SupervisorId = dto.SupervisorId,
        };

        await repository.AddAsync(report, ct);
        await repository.SaveChangesAsync(ct);

        // Recarrega com as relações incluídas, igual ao padrão de JobOpeningService.CreateAsync.
        var created = await repository.GetByIdAsync(report.Id, ct) ?? report;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateReportDto dto, CancellationToken ct = default)
    {
        var report = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"Report {dto.Id} not found.");

        report.ReportTemplateId = dto.ReportTemplateId;
        report.Descriptive = dto.Descriptive;
        report.FileName = dto.FileName;
        report.ResponsibleId = dto.ResponsibleId;
        report.SupervisorId = dto.SupervisorId;
        report.ResponsibleSignatureId = dto.ResponsibleSignatureId;
        report.SupervisorSignatureId = dto.SupervisorSignatureId;
        report.Utilization = dto.Utilization;
        report.Average = dto.Average;
        report.UpdatedById = dto.UpdatedById;
        report.UpdatedAt = DateTime.UtcNow;

        repository.Update(report);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var report = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Report {id} not found.");

        repository.Remove(report);
        await repository.SaveChangesAsync(ct);
    }

    private static ReportDto ToDto(Report r) => new(
        r.Id, r.AssessmentEventId,
        r.AssessmentEvent?.Candidate?.Name ?? "(unknown)",
        r.ReportTemplateId, r.ReportTemplate?.Name ?? "(unknown)",
        r.Descriptive, r.FileName, r.FileCreatedAt,
        r.ResponsibleId, r.Responsible?.Name ?? "(unknown)",
        r.SupervisorId, r.Supervisor?.Name,
        r.ResponsibleSignatureId, r.ResponsibleSignature?.Name,
        r.SupervisorSignatureId, r.SupervisorSignature?.Name,
        r.Utilization, r.Average, r.UpdatedAt, r.CreatedAt);
}
