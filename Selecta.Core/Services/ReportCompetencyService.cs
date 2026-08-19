using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ReportCompetencyService(IReportCompetencyRepository repository) : IReportCompetencyService
{
    public async Task<IEnumerable<ReportCompetencyDto>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await repository.GetAllAsync(ct);
        return items.Select(ToDto);
    }

    public async Task<IEnumerable<ReportCompetencyDto>> GetByReportAsync(Guid reportId, CancellationToken ct = default)
    {
        var items = await repository.GetByReportAsync(reportId, ct);
        return items.Select(ToDto);
    }

    public async Task<PagedResult<ReportCompetencyDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ReportCompetencyDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ReportCompetencyDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var item = await repository.GetByIdAsync(id, ct);
        return item is null ? null : ToDto(item);
    }

    public async Task<ReportCompetencyDto> CreateAsync(CreateReportCompetencyDto dto, CancellationToken ct = default)
    {
        var item = new ReportCompetency
        {
            ReportId = dto.ReportId,
            CompetencyId = dto.CompetencyId,
            CompetencyDescriptorId = dto.CompetencyDescriptorId,
            ProfileScoreId = dto.ProfileScoreId,
            ScoreId = dto.ScoreId,
            Percentage = dto.Percentage,
        };

        await repository.AddAsync(item, ct);
        await repository.SaveChangesAsync(ct);

        // Recarrega com as relações incluídas, igual ao padrão de JobOpeningService.CreateAsync.
        var created = await repository.GetByIdAsync(item.Id, ct) ?? item;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateReportCompetencyDto dto, CancellationToken ct = default)
    {
        var item = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"ReportCompetency {dto.Id} not found.");

        item.ReportId = dto.ReportId;
        item.CompetencyId = dto.CompetencyId;
        item.CompetencyDescriptorId = dto.CompetencyDescriptorId;
        item.ProfileScoreId = dto.ProfileScoreId;
        item.ScoreId = dto.ScoreId;
        item.Percentage = dto.Percentage;

        repository.Update(item);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var item = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"ReportCompetency {id} not found.");

        repository.Remove(item);
        await repository.SaveChangesAsync(ct);
    }

    private static ReportCompetencyDto ToDto(ReportCompetency c) => new(
        c.Id, c.ReportId,
        c.CompetencyId, c.Competency?.Name ?? "(unknown)",
        c.CompetencyDescriptorId, c.CompetencyDescriptor?.Name,
        c.ProfileScoreId, c.ProfileScore?.Name,
        c.ScoreId, c.Score?.Name,
        c.Percentage);
}
