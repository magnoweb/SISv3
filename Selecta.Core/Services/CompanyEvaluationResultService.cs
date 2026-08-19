using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class CompanyEvaluationResultService(ICompanyEvaluationResultRepository repository) : ICompanyEvaluationResultService
{
    public async Task<IEnumerable<CompanyEvaluationResultDto>> GetAllAsync(CancellationToken ct = default)
    {
        var results = await repository.GetAllAsync(ct);
        return results.Select(ToDto);
    }

    public async Task<PagedResult<CompanyEvaluationResultDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<CompanyEvaluationResultDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<IEnumerable<CompanyEvaluationResultDto>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default)
    {
        var results = await repository.GetByCompanyAsync(companyId, ct);
        return results.Select(ToDto);
    }

    public async Task<CompanyEvaluationResultDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var result = await repository.GetByIdAsync(id, ct);
        return result is null ? null : ToDto(result);
    }

    public async Task<CompanyEvaluationResultDto> CreateAsync(CreateCompanyEvaluationResultDto dto, CancellationToken ct = default)
    {
        var result = new CompanyEvaluationResult { EvaluationResultId = dto.EvaluationResultId, CompanyId = dto.CompanyId, Name = dto.Name };
        await repository.AddAsync(result, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(result);
    }

    public async Task UpdateAsync(UpdateCompanyEvaluationResultDto dto, CancellationToken ct = default)
    {
        var result = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"CompanyEvaluationResult {dto.Id} not found.");

        result.EvaluationResultId = dto.EvaluationResultId;
        result.CompanyId = dto.CompanyId;
        result.Name = dto.Name;

        repository.Update(result);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var result = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"CompanyEvaluationResult {id} not found.");

        repository.Remove(result);
        await repository.SaveChangesAsync(ct);
    }

    private static CompanyEvaluationResultDto ToDto(CompanyEvaluationResult r) => new(r.Id, r.EvaluationResultId, r.CompanyId, r.Name);
}
