using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ReportComponentApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ReportComponentDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ReportComponentDto>>("api/reportcomponents") ?? [];
    }

    public async Task<PagedResult<ReportComponentDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ReportComponentDto>>($"api/reportcomponents/paged?{query}")
            ?? new PagedResult<ReportComponentDto>([], 0, page, pageSize);
    }

    public async Task<ReportComponentDto> CreateAsync(CreateReportComponentDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/reportcomponents", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ReportComponentDto>())!;
    }

    public async Task UpdateAsync(UpdateReportComponentDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/reportcomponents/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/reportcomponents/{id}");
        response.EnsureSuccessStatusCode();
    }

    private async Task AuthenticateAsync()
    {
        var token = await tokenStorage.GetAsync();
        http.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
    }
}
