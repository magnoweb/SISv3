using Microsoft.EntityFrameworkCore;
using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Validation;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class CandidateRepository(SelectaDbContext context) : RepositoryBase<Candidate>(context), ICandidateRepository
{
    protected override string DefaultOrderBy => "Name";

    public async Task<Candidate?> GetByCpfAsync(string cpf, CancellationToken ct = default) =>
        await DbSet.FirstOrDefaultAsync(c => c.Cpf == cpf, ct);

    public async Task<Candidate?> SearchByNameOrCpfAsync(string term, CancellationToken ct = default) =>
        CpfValidator.IsValid(term)
            ? await DbSet.FirstOrDefaultAsync(c => c.Cpf == term, ct)
            : await DbSet.FirstOrDefaultAsync(c => c.Name.Contains(term), ct);
}
