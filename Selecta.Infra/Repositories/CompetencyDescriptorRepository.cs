using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class CompetencyDescriptorRepository(SelectaDbContext context) : RepositoryBase<CompetencyDescriptor>(context), ICompetencyDescriptorRepository
{
    protected override string DefaultOrderBy => "CreatedAt desc";

    private IQueryable<CompetencyDescriptor> WithRelated() => DbSet.AsNoTracking()
        .Include(d => d.Competency)
        .Include(d => d.User);

    public override async Task<CompetencyDescriptor?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(d => d.Id == id, ct);

    public override async Task<IEnumerable<CompetencyDescriptor>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public override Task<PagedResult<CompetencyDescriptor>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
