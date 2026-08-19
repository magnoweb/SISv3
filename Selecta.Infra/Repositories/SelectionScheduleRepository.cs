using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Schedule;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class SelectionScheduleRepository(SelectaDbContext context) : RepositoryBase<SelectionSchedule>(context), ISelectionScheduleRepository
{
    protected override string DefaultOrderBy => "Date desc";

    private IQueryable<SelectionSchedule> WithRelated() => DbSet.AsNoTracking()
        .Include(s => s.JobTitle)
        .Include(s => s.Contact)
        .Include(s => s.CreatedBy);

    public override async Task<SelectionSchedule?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(s => s.Id == id, ct);

    public override async Task<IEnumerable<SelectionSchedule>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public override Task<PagedResult<SelectionSchedule>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);

    public async Task<bool> HasPriorEntriesAsync(string cpf, CancellationToken ct = default) =>
        await DbSet.AsNoTracking().AnyAsync(s => s.Cpf == cpf, ct);
}
