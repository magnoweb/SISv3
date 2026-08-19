using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class OpinionListService(IOpinionListRepository repository) : IOpinionListService
{
    public async Task<IEnumerable<OpinionListDto>> GetAllAsync(CancellationToken ct = default)
    {
        var lists = await repository.GetAllAsync(ct);
        return lists.Select(ToDto);
    }

    public async Task<PagedResult<OpinionListDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<OpinionListDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<OpinionListDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var list = await repository.GetByIdAsync(id, ct);
        return list is null ? null : ToDto(list);
    }

    public async Task<OpinionListDto> CreateAsync(CreateOpinionListDto dto, CancellationToken ct = default)
    {
        var list = new OpinionList
        {
            // Gerado no servidor — formato "yyyyMMdd_HHmmss", confirmado nas capturas de tela da v2 (coluna tem 20 caracteres).
            Code = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss"),
            ContactId = dto.ContactId,
            ResponsibleId = dto.ResponsibleId,
            Date = dto.Date,
            Notes = dto.Notes,
            CreatedById = dto.CreatedById,
        };

        await repository.AddAsync(list, ct);
        await repository.SaveChangesAsync(ct);

        var created = await repository.GetByIdAsync(list.Id, ct) ?? list;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateOpinionListDto dto, CancellationToken ct = default)
    {
        var list = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"OpinionList {dto.Id} not found.");

        list.ContactId = dto.ContactId;
        list.ResponsibleId = dto.ResponsibleId;
        list.Date = dto.Date;
        list.Notes = dto.Notes;

        repository.Update(list);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var list = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"OpinionList {id} not found.");

        repository.Remove(list);
        await repository.SaveChangesAsync(ct);
    }

    private static OpinionListDto ToDto(OpinionList l) => new(
        l.Id, l.Code,
        l.ContactId, l.Contact?.Name ?? "(unknown)",
        l.ResponsibleId, l.Responsible?.Name ?? "(unknown)",
        l.Date, l.Notes,
        l.CreatedById, l.CreatedBy?.Name ?? "(unknown)",
        l.CreatedAt);
}
