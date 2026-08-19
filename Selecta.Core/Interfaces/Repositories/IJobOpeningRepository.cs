using Selecta.Core.Dtos;
using Selecta.Core.Entities.Recruitment;

namespace Selecta.Core.Interfaces.Repositories;

public interface IJobOpeningRepository : IRepositoryBase<JobOpening>
{
    /// <summary>Vagas com status Novo, Em Andamento ou Em Reposição (réplica parcial de ObterVagasAtivasPorEmpresa,
    /// sem o filtro por empresa — esse depende do módulo Contact/Company, ainda não portado).</summary>
    Task<IEnumerable<JobOpening>> GetActiveAsync(CancellationToken ct = default);

    /// <summary>Sobrecarga com filtro "apenas ativas" — mantém o TotalCount correto quando o filtro está ligado.</summary>
    Task<PagedResult<JobOpening>> GetPagedAsync(int page, int pageSize, bool activeOnly, string? filter = null, string? orderBy = null, CancellationToken ct = default);
}
