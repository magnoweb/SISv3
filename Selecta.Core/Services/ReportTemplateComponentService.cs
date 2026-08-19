using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ReportTemplateComponentService(IReportTemplateComponentRepository repository) : IReportTemplateComponentService
{
    public async Task<IEnumerable<ReportTemplateComponentDto>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await repository.GetAllAsync(ct);
        return items.Select(ToDto);
    }

    public async Task<IEnumerable<ReportTemplateComponentDto>> GetByReportTemplateAsync(int reportTemplateId, CancellationToken ct = default)
    {
        var items = await repository.GetByReportTemplateAsync(reportTemplateId, ct);
        return items.Select(ToDto);
    }

    public async Task<PagedResult<ReportTemplateComponentDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ReportTemplateComponentDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ReportTemplateComponentDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var item = await repository.GetByIdAsync(id, ct);
        return item is null ? null : ToDto(item);
    }

    public async Task<ReportTemplateComponentDto> CreateAsync(CreateReportTemplateComponentDto dto, CancellationToken ct = default)
    {
        var item = new ReportTemplateComponent { ReportTemplateId = dto.ReportTemplateId, ReportComponentId = dto.ReportComponentId };
        await repository.AddAsync(item, ct);
        await repository.SaveChangesAsync(ct);

        var created = await repository.GetByIdAsync(item.Id, ct) ?? item;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateReportTemplateComponentDto dto, CancellationToken ct = default)
    {
        var item = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"ReportTemplateComponent {dto.Id} not found.");

        item.ReportTemplateId = dto.ReportTemplateId;
        item.ReportComponentId = dto.ReportComponentId;

        repository.Update(item);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var item = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"ReportTemplateComponent {id} not found.");

        repository.Remove(item);
        await repository.SaveChangesAsync(ct);
    }

    private static ReportTemplateComponentDto ToDto(ReportTemplateComponent c) => new(
        c.Id, c.ReportTemplateId, c.ReportTemplate?.Name ?? "(unknown)",
        c.ReportComponentId, c.ReportComponent?.Name ?? "(unknown)");
}
