using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class ProfessionalGroupCompetencyProfileRepository(SelectaDbContext context) : RepositoryBase<ProfessionalGroupCompetencyProfile>(context), IProfessionalGroupCompetencyProfileRepository
{
    protected override string DefaultOrderBy => "Name";

    private IQueryable<ProfessionalGroupCompetencyProfile> WithRelated() => DbSet.AsNoTracking().Include(p => p.ProfessionalGroup);

    public override async Task<ProfessionalGroupCompetencyProfile?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(p => p.Id == id, ct);

    public override async Task<IEnumerable<ProfessionalGroupCompetencyProfile>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public override Task<PagedResult<ProfessionalGroupCompetencyProfile>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);
}
