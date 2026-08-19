using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ReportApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ReportDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ReportDto>>("api/reports") ?? [];
    }

    public async Task<PagedResult<ReportDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ReportDto>>($"api/reports/paged?{query}")
            ?? new PagedResult<ReportDto>([], 0, page, pageSize);
    }

    public async Task<ReportDto> CreateAsync(CreateReportDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/reports", dto);
        await EnsureSuccessOrThrowDomainMessageAsync(response);
        return (await response.Content.ReadFromJsonAsync<ReportDto>())!;
    }

    public async Task UpdateAsync(UpdateReportDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/reports/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/reports/{id}");
        response.EnsureSuccessStatusCode();
    }

    private async Task AuthenticateAsync()
    {
        var token = await tokenStorage.GetAsync();
        http.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
    }

    private static async Task EnsureSuccessOrThrowDomainMessageAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var problem = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            throw new InvalidOperationException(problem?.GetValueOrDefault("message") ?? "Dados inválidos.");
        }

        response.EnsureSuccessStatusCode();
    }
}
