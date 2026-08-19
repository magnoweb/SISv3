using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Repositories;

/// <summary>
/// Não segue o padrão IRepositoryBase&lt;T&gt; porque não há uma entidade
/// própria — é uma consulta agregada sobre várias tabelas. A implementação
/// em Selecta.Infra usa Dapper/SQL direto em vez do EF Core, propositadamente.
/// </summary>
public interface IDashboardRepository
{
    Task<DashboardSummaryDto> GetSummaryAsync(CancellationToken ct = default);
}
