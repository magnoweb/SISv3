using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class OpinionListRepository(SelectaDbContext context) : RepositoryBase<OpinionList>(context), IOpinionListRepository
{
    protected override string DefaultOrderBy => "Date desc";

    private IQueryable<OpinionList> WithRelated() => DbSet.AsNoTracking()
        .Include(l => l.Contact)
        .Include(l => l.Responsible)
        .Include(l => l.CreatedBy);

    public override async Task<OpinionList?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(l => l.Id == id, ct);

    public override async Task<IEnumerable<OpinionList>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public override Task<PagedResult<OpinionList>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
