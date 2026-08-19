using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ReportTemplateService(IReportTemplateRepository repository) : IReportTemplateService
{
    public async Task<IEnumerable<ReportTemplateDto>> GetAllAsync(CancellationToken ct = default)
    {
        var templates = await repository.GetAllAsync(ct);
        return templates.Select(ToDto);
    }

    public async Task<PagedResult<ReportTemplateDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ReportTemplateDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ReportTemplateDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var template = await repository.GetByIdAsync(id, ct);
        return template is null ? null : ToDto(template);
    }

    public async Task<ReportTemplateDto> CreateAsync(CreateReportTemplateDto dto, CancellationToken ct = default)
    {
        var template = new ReportTemplate
        {
            Name = dto.Name,
            Template = dto.Template,
            ProductionActivityId = dto.ProductionActivityId,
            ReadingActivityId = dto.ReadingActivityId,
            HeaderId = dto.HeaderId,
            FooterId = dto.FooterId,
            AttachmentReport = dto.AttachmentReport,
            UseCompetencies = dto.UseCompetencies,
        };

        await repository.AddAsync(template, ct);
        await repository.SaveChangesAsync(ct);

        // Recarrega com as relações incluídas, igual ao padrão de JobOpeningService.CreateAsync.
        var created = await repository.GetByIdAsync(template.Id, ct) ?? template;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateReportTemplateDto dto, CancellationToken ct = default)
    {
        var template = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"ReportTemplate {dto.Id} not found.");

        template.Name = dto.Name;
        template.Template = dto.Template;
        template.ProductionActivityId = dto.ProductionActivityId;
        template.ReadingActivityId = dto.ReadingActivityId;
        template.HeaderId = dto.HeaderId;
        template.FooterId = dto.FooterId;
        template.AttachmentReport = dto.AttachmentReport;
        template.UseCompetencies = dto.UseCompetencies;
        template.Active = dto.Active;

        repository.Update(template);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var template = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"ReportTemplate {id} not found.");

        repository.Remove(template);
        await repository.SaveChangesAsync(ct);
    }

    private static ReportTemplateDto ToDto(ReportTemplate t) => new(
        t.Id, t.Name, t.Template,
        t.ProductionActivityId, t.ProductionActivity?.Name ?? "(unknown)",
        t.ReadingActivityId, t.ReadingActivity?.Name ?? "(unknown)",
        t.HeaderId, t.Header?.Name,
        t.FooterId, t.Footer?.Name,
        t.AttachmentReport, t.UseCompetencies, t.Active);
}
