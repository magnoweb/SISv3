using Selecta.Core.Dtos;
using Selecta.Core.Entities;

namespace Selecta.Core.Interfaces;

/// <summary>
/// Contrato genérico de acesso a dados, implementado em Selecta.Infra.
/// Substitui a antiga cadeia IRepositoryBase + IServiceBase + IAppServiceBase
/// (praticamente idênticas na solução original) por um único contrato,
/// assíncrono, como é o padrão em EF Core. TKey existe por causa de algumas
/// tabelas legadas com chave int (ex.: "Atividades") — a esmagadora maioria
/// usa Guid e continua a implementar apenas <see cref="IRepositoryBase{TEntity}"/>.
/// </summary>
public interface IRepositoryBase<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Página 1-based. Usada pelas telas de listagem (RadzenDataGrid) — GetAllAsync
    /// continua disponível para popular dropdowns/lookups. <paramref name="filter"/> e
    /// <paramref name="orderBy"/> são expressões Dynamic LINQ (System.Linq.Dynamic.Core),
    /// geradas automaticamente pelo RadzenDataGrid a partir dos filtros/cabeçalhos
    /// clicados pelo utilizador — null/vazio aplica o comportamento por omissão.
    /// </summary>
    Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);

    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

/// <summary>Atalho para o caso mais comum — chave Guid. Todos os módulos exceto os de chave legada int usam este.</summary>
public interface IRepositoryBase<TEntity> : IRepositoryBase<TEntity, Guid> where TEntity : class, IEntity;
