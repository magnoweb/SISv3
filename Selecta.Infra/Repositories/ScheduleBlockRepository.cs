using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Schedule;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ScheduleBlockRepository(SelectaDbContext context) : RepositoryBase<ScheduleBlock>(context), IScheduleBlockRepository
{
    protected override string DefaultOrderBy => "Date desc";

    private IQueryable<ScheduleBlock> WithRelated() => DbSet.AsNoTracking().Include(b => b.User);

    public override async Task<ScheduleBlock?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(b => b.Id == id, ct);

    public override async Task<IEnumerable<ScheduleBlock>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public override Task<PagedResult<ScheduleBlock>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
