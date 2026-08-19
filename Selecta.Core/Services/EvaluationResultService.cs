using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class EvaluationResultService(IEvaluationResultRepository repository) : IEvaluationResultService
{
    public async Task<IEnumerable<EvaluationResultDto>> GetAllAsync(CancellationToken ct = default)
    {
        var results = await repository.GetAllAsync(ct);
        return results.Select(ToDto);
    }

    public async Task<PagedResult<EvaluationResultDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<EvaluationResultDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<EvaluationResultDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var result = await repository.GetByIdAsync(id, ct);
        return result is null ? null : ToDto(result);
    }

    public async Task<EvaluationResultDto> CreateAsync(CreateEvaluationResultDto dto, CancellationToken ct = default)
    {
        var result = new EvaluationResult { Name = dto.Name, Value = dto.Value, CssClass = dto.CssClass };
        await repository.AddAsync(result, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(result);
    }

    public async Task UpdateAsync(UpdateEvaluationResultDto dto, CancellationToken ct = default)
    {
        var result = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"EvaluationResult {dto.Id} not found.");

        result.Name = dto.Name;
        result.Value = dto.Value;
        result.CssClass = dto.CssClass;

        repository.Update(result);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var result = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"EvaluationResult {id} not found.");

        repository.Remove(result);
        await repository.SaveChangesAsync(ct);
    }

    private static EvaluationResultDto ToDto(EvaluationResult r) => new(r.Id, r.Name, r.Value, r.CssClass);
}
