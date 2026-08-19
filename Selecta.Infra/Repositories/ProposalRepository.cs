using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Administrative;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ProposalRepository(SelectaDbContext context) : RepositoryBase<Proposal>(context), IProposalRepository
{
    protected override string DefaultOrderBy => "CreatedAt desc";

    private IQueryable<Proposal> WithRelated() => DbSet.AsNoTracking()
        .Include(p => p.ServiceOffering)
        .Include(p => p.ProspectCompany);

    public override async Task<Proposal?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(p => p.Id == id, ct);

    public override async Task<IEnumerable<Proposal>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public override Task<PagedResult<Proposal>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
