using Microsoft.EntityFrameworkCore;
using Selecta.Core.Dtos;
using Selecta.Core.Entities.Enums;
using Selecta.Core.Entities.Recruitment;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class JobOpeningRepository(SelectaDbContext context) : RepositoryBase<JobOpening>(context), IJobOpeningRepository
{
    protected override string DefaultOrderBy => "CreatedAt desc";

    private static readonly JobOpeningStatus[] ActiveStatuses =
    [
        JobOpeningStatus.New,
        JobOpeningStatus.InProgress,
        JobOpeningStatus.InReplacement,
    ];

    private IQueryable<JobOpening> WithRelated() => DbSet.AsNoTracking()
        .Include(j => j.Manager)
        .Include(j => j.Contact)
        .Include(j => j.JobTitle)
        .Include(j => j.RecruitmentStage);

    public override async Task<JobOpening?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await WithRelated().FirstOrDefaultAsync(j => j.Id == id, ct);

    public override async Task<IEnumerable<JobOpening>> GetAllAsync(CancellationToken ct = default) =>
        await WithRelated().ToListAsync(ct);

    public async Task<IEnumerable<JobOpening>> GetActiveAsync(CancellationToken ct = default) =>
        await WithRelated().Where(j => ActiveStatuses.Contains(j.Status)).ToListAsync(ct);

    public override Task<PagedResult<JobOpening>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        PageAsync(WithRelated(), page, pageSize, filter, orderBy, ct);

    public Task<PagedResult<JobOpening>> GetPagedAsync(
        int page, int pageSize, bool activeOnly, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var query = activeOnly ? WithRelated().Where(j => ActiveStatuses.Contains(j.Status)) : WithRelated();
        return PageAsync(query, page, pageSize, filter, orderBy, ct);
    }
}
