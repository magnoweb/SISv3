using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ReportTemplateApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ReportTemplateDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ReportTemplateDto>>("api/reporttemplates") ?? [];
    }

    public async Task<PagedResult<ReportTemplateDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ReportTemplateDto>>($"api/reporttemplates/paged?{query}")
            ?? new PagedResult<ReportTemplateDto>([], 0, page, pageSize);
    }

    public async Task<ReportTemplateDto> CreateAsync(CreateReportTemplateDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/reporttemplates", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ReportTemplateDto>())!;
    }

    public async Task UpdateAsync(UpdateReportTemplateDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/reporttemplates/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/reporttemplates/{id}");
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
