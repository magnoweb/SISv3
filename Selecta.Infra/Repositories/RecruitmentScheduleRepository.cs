using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Schedule;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class RecruitmentScheduleRepository(SelectaDbContext context) : RepositoryBase<RecruitmentSchedule>(context), IRecruitmentScheduleRepository
{
    protected override string DefaultOrderBy => "Date desc";

    private IQueryable<RecruitmentSchedule> WithRelated() => DbSet.AsNoTracking()
        .Include(s => s.JobOpening)
        .Include(s => s.Responsible)
        .Include(s => s.CreatedBy);

    public override async Task<RecruitmentSchedule?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(s => s.Id == id, ct);

    public override async Task<IEnumerable<RecruitmentSchedule>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public override Task<PagedResult<RecruitmentSchedule>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);

    public async Task<bool> HasPriorEntriesAsync(string cpf, CancellationToken ct = default) =>
        await DbSet.AsNoTracking().AnyAsync(s => s.Cpf == cpf, ct);
}
