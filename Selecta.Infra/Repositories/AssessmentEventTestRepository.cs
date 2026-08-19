using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class AssessmentEventTestRepository(SelectaDbContext context) : RepositoryBase<AssessmentEventTest>(context), IAssessmentEventTestRepository
{
    private IQueryable<AssessmentEventTest> WithRelated() => DbSet.AsNoTracking()
        .Include(t => t.AssessmentEvent!).ThenInclude(e => e!.Candidate)
        .Include(t => t.PsychologicalTest);

    public override async Task<AssessmentEventTest?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(t => t.Id == id, ct);

    public override async Task<IEnumerable<AssessmentEventTest>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public async Task<IEnumerable<AssessmentEventTest>> GetByAssessmentEventAsync(Guid assessmentEventId, CancellationToken ct = default) =>
        await WithRelated().Where(t => t.AssessmentEventId == assessmentEventId).ToListAsync(ct);

    public override Task<PagedResult<AssessmentEventTest>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
