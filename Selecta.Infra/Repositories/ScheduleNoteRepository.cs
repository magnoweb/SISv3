using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Schedule;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ScheduleNoteRepository(SelectaDbContext context) : RepositoryBase<ScheduleNote>(context), IScheduleNoteRepository
{
    protected override string DefaultOrderBy => "Date desc";

    private IQueryable<ScheduleNote> WithRelated() => DbSet.AsNoTracking().Include(n => n.CreatedBy);

    public override async Task<ScheduleNote?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(n => n.Id == id, ct);

    public override async Task<IEnumerable<ScheduleNote>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public override Task<PagedResult<ScheduleNote>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
