namespace Selecta.Core.Dtos;

/// <summary>
/// Resultado paginado genérico — usado tanto internamente (entidades, entre
/// Infra e Core) quanto como retorno público da Api (DTOs). Page é 1-based.
/// </summary>
public record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize)
{
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}
