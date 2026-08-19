using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ReportComponentService(IReportComponentRepository repository) : IReportComponentService
{
    public async Task<IEnumerable<ReportComponentDto>> GetAllAsync(CancellationToken ct = default)
    {
        var components = await repository.GetAllAsync(ct);
        return components.Select(ToDto);
    }

    public async Task<PagedResult<ReportComponentDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ReportComponentDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ReportComponentDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var component = await repository.GetByIdAsync(id, ct);
        return component is null ? null : ToDto(component);
    }

    public async Task<ReportComponentDto> CreateAsync(CreateReportComponentDto dto, CancellationToken ct = default)
    {
        var component = new ReportComponent
        {
            ComponentType = dto.ComponentType,
            Name = dto.Name,
            Tag = dto.Tag,
            FileName = dto.FileName,
            Content = dto.Content,
        };

        await repository.AddAsync(component, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(component);
    }

    public async Task UpdateAsync(UpdateReportComponentDto dto, CancellationToken ct = default)
    {
        var component = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"ReportComponent {dto.Id} not found.");

        component.ComponentType = dto.ComponentType;
        component.Name = dto.Name;
        component.Tag = dto.Tag;
        component.FileName = dto.FileName;
        component.Content = dto.Content;
        component.Active = dto.Active;

        repository.Update(component);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var component = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"ReportComponent {id} not found.");

        repository.Remove(component);
        await repository.SaveChangesAsync(ct);
    }

    private static ReportComponentDto ToDto(ReportComponent c) => new(c.Id, c.ComponentType, c.Name, c.Tag, c.FileName, c.Content, c.Active);
}
