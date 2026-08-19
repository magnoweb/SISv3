using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class AssessmentEventRepository(SelectaDbContext context) : RepositoryBase<AssessmentEvent>(context), IAssessmentEventRepository
{
    protected override string DefaultOrderBy => "Date desc";

    private IQueryable<AssessmentEvent> WithRelated() => DbSet.AsNoTracking()
        .Include(e => e.Candidate)
        .Include(e => e.JobTitle)
        .Include(e => e.Contact)
        .Include(e => e.EvaluationResult);

    public override async Task<AssessmentEvent?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(e => e.Id == id, ct);

    public override async Task<IEnumerable<AssessmentEvent>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public override Task<PagedResult<AssessmentEvent>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
