namespace Selecta.Web.Services;

/// <summary>
/// Monta a query string dos endpoints /paged. filter/orderBy são expressões
/// Dynamic LINQ geradas pelo RadzenDataGrid (ex.: <c>Name.Contains("abc")</c>,
/// <c>Name desc</c>) — contêm aspas, parênteses e espaços, por isso precisam
/// de Uri.EscapeDataString antes de ir para a URL.
/// </summary>
internal static class PagedQueryBuilder
{
    public static string Build(int page, int pageSize, string? filter, string? orderBy)
    {
        var query = $"page={page}&pageSize={pageSize}";

        if (!string.IsNullOrWhiteSpace(filter))
            query += $"&filter={Uri.EscapeDataString(filter)}";

        if (!string.IsNullOrWhiteSpace(orderBy))
            query += $"&orderBy={Uri.EscapeDataString(orderBy)}";

        return query;
    }
}
