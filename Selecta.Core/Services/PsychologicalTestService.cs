using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class PsychologicalTestService(IPsychologicalTestRepository repository) : IPsychologicalTestService
{
    public async Task<IEnumerable<PsychologicalTestDto>> GetAllAsync(CancellationToken ct = default)
    {
        var tests = await repository.GetAllAsync(ct);
        return tests.Select(ToDto);
    }

    public async Task<PagedResult<PsychologicalTestDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<PsychologicalTestDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<PsychologicalTestDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var test = await repository.GetByIdAsync(id, ct);
        return test is null ? null : ToDto(test);
    }

    public async Task<PsychologicalTestDto> CreateAsync(CreatePsychologicalTestDto dto, CancellationToken ct = default)
    {
        var test = new PsychologicalTest { Name = dto.Name, Description = dto.Description };
        await repository.AddAsync(test, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(test);
    }

    public async Task UpdateAsync(UpdatePsychologicalTestDto dto, CancellationToken ct = default)
    {
        var test = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"PsychologicalTest {dto.Id} not found.");

        test.Name = dto.Name;
        test.Description = dto.Description;
        test.Active = dto.Active;

        repository.Update(test);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var test = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"PsychologicalTest {id} not found.");

        repository.Remove(test);
        await repository.SaveChangesAsync(ct);
    }

    private static PsychologicalTestDto ToDto(PsychologicalTest t) => new(t.Id, t.Name, t.Description, t.Active, t.CreatedAt);
}
