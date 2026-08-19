using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Selecta.Core.Entities;
using Selecta.Core.Interfaces;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

/// <summary>
/// Implementação genérica real (chave TKey). A esmagadora maioria dos
/// repositórios estende <see cref="RepositoryBase{TEntity}"/> (o atalho
/// Guid, logo abaixo) — só entidades com chave legada int (ex.: Activity)
/// estendem esta versão diretamente.
/// </summary>
public class RepositoryBase<TEntity, TKey>(SelectaDbContext context) : IRepositoryBase<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    protected readonly SelectaDbContext Context = context;
    protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

    private const int MaxPageSize = 200;

    public async Task AddAsync(TEntity entity, CancellationToken ct = default) =>
        await DbSet.AddAsync(entity, ct);

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default) =>
        await DbSet.FindAsync([id], ct);

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default) =>
        await DbSet.AsNoTracking().ToListAsync(ct);

    /// <summary>
    /// Expressão Dynamic LINQ usada quando o pedido não especifica ordenação
    /// (primeiro carregamento do grid, antes do utilizador clicar num
    /// cabeçalho de coluna). Sem ORDER BY explícito o SQL Server não garante
    /// ordem estável entre páginas — por isso o valor por omissão é "Id", e
    /// repositórios específicos sobrescrevem para algo mais útil (ex.: "Name",
    /// "Order", "CreatedAt desc") — ver CityRepository, JobOpeningRepository, etc.
    /// </summary>
    protected virtual string DefaultOrderBy => "Id";

    /// <summary>
    /// Página 1-based, com filtro e ordenação dinâmicos (strings geradas pelo
    /// RadzenDataGrid via System.Linq.Dynamic.Core — ex.: filter:
    /// <c>Name.Contains("abc")</c>, orderBy: <c>Name desc</c>). Repositórios com
    /// Include (JobOpening, ScheduleBlock, Proposal) sobrescrevem para aplicar
    /// o filtro sobre a query já incluída — ver PageAsync.
    /// </summary>
    public virtual async Task<Selecta.Core.Dtos.PagedResult<TEntity>> GetPagedAsync(
        int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default) =>
        await PageAsync(DbSet.AsNoTracking(), page, pageSize, filter, orderBy, ct);

    protected async Task<Selecta.Core.Dtos.PagedResult<TEntity>> PageAsync(
        IQueryable<TEntity> baseQuery, int page, int pageSize, string? filter, string? orderBy, CancellationToken ct)
    {
        var query = ApplyFilter(baseQuery, filter);
        query = query.OrderBy(string.IsNullOrWhiteSpace(orderBy) ? DefaultOrderBy : orderBy);

        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new Selecta.Core.Dtos.PagedResult<TEntity>(items, totalCount, page, pageSize);
    }

    /// <summary>
    /// Algumas colunas do grid vêm de DTOs com campos computados/resolvidos por
    /// navegação (ex.: JobOpeningDto.ManagerName) que não existem na entidade —
    /// essas colunas já ficam marcadas Filterable="false" no Blazor, mas por
    /// segurança um filtro que a Dynamic LINQ não consiga interpretar é
    /// ignorado aqui em vez de derrubar o pedido com 500.
    /// </summary>
    private static IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query, string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter)) return query;

        try
        {
            return query.Where(filter);
        }
        catch (Exception)
        {
            return query;
        }
    }

    public void Update(TEntity entity) => Context.Entry(entity).State = EntityState.Modified;

    public void Remove(TEntity entity) => DbSet.Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => Context.SaveChangesAsync(ct);
}

/// <summary>Atalho para o caso mais comum — chave Guid. Quase todos os repositórios estendem este.</summary>
public class RepositoryBase<TEntity>(SelectaDbContext context) : RepositoryBase<TEntity, Guid>(context), IRepositoryBase<TEntity>
    where TEntity : class, IEntity;
