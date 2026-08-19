using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class AssessmentEventTestService(IAssessmentEventTestRepository repository) : IAssessmentEventTestService
{
    public async Task<IEnumerable<AssessmentEventTestDto>> GetAllAsync(CancellationToken ct = default)
    {
        var tests = await repository.GetAllAsync(ct);
        return tests.Select(ToDto);
    }

    public async Task<IEnumerable<AssessmentEventTestDto>> GetByAssessmentEventAsync(Guid assessmentEventId, CancellationToken ct = default)
    {
        var tests = await repository.GetByAssessmentEventAsync(assessmentEventId, ct);
        return tests.Select(ToDto);
    }

    public async Task<PagedResult<AssessmentEventTestDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<AssessmentEventTestDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<AssessmentEventTestDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var test = await repository.GetByIdAsync(id, ct);
        return test is null ? null : ToDto(test);
    }

    public async Task<AssessmentEventTestDto> CreateAsync(CreateAssessmentEventTestDto dto, CancellationToken ct = default)
    {
        var test = new AssessmentEventTest
        {
            AssessmentEventId = dto.AssessmentEventId,
            PsychologicalTestId = dto.PsychologicalTestId,
            Percentage = dto.Percentage,
        };

        await repository.AddAsync(test, ct);
        await repository.SaveChangesAsync(ct);

        var created = await repository.GetByIdAsync(test.Id, ct) ?? test;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateAssessmentEventTestDto dto, CancellationToken ct = default)
    {
        var test = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"AssessmentEventTest {dto.Id} not found.");

        test.AssessmentEventId = dto.AssessmentEventId;
        test.PsychologicalTestId = dto.PsychologicalTestId;
        test.Percentage = dto.Percentage;

        repository.Update(test);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var test = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"AssessmentEventTest {id} not found.");

        repository.Remove(test);
        await repository.SaveChangesAsync(ct);
    }

    private static AssessmentEventTestDto ToDto(AssessmentEventTest t) => new(
        t.Id, t.AssessmentEventId, t.AssessmentEvent?.Candidate?.Name ?? "(unknown)",
        t.PsychologicalTestId, t.PsychologicalTest?.Name ?? "(unknown)",
        t.Percentage);
}
