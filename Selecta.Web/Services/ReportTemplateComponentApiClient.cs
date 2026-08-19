using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ReportTemplateComponentApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ReportTemplateComponentDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ReportTemplateComponentDto>>("api/reporttemplatecomponents") ?? [];
    }

    public async Task<PagedResult<ReportTemplateComponentDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ReportTemplateComponentDto>>($"api/reporttemplatecomponents/paged?{query}")
            ?? new PagedResult<ReportTemplateComponentDto>([], 0, page, pageSize);
    }

    public async Task<ReportTemplateComponentDto> CreateAsync(CreateReportTemplateComponentDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/reporttemplatecomponents", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ReportTemplateComponentDto>())!;
    }

    public async Task UpdateAsync(UpdateReportTemplateComponentDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/reporttemplatecomponents/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/reporttemplatecomponents/{id}");
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
