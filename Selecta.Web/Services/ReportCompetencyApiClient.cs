using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ReportCompetencyApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ReportCompetencyDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ReportCompetencyDto>>("api/reportcompetencies") ?? [];
    }

    public async Task<PagedResult<ReportCompetencyDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ReportCompetencyDto>>($"api/reportcompetencies/paged?{query}")
            ?? new PagedResult<ReportCompetencyDto>([], 0, page, pageSize);
    }

    public async Task<ReportCompetencyDto> CreateAsync(CreateReportCompetencyDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/reportcompetencies", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ReportCompetencyDto>())!;
    }

    public async Task UpdateAsync(UpdateReportCompetencyDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/reportcompetencies/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/reportcompetencies/{id}");
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
