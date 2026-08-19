using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class CompetencyApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<CompetencyDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<CompetencyDto>>("api/competencies") ?? [];
    }

    public async Task<PagedResult<CompetencyDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<CompetencyDto>>($"api/competencies/paged?{query}")
            ?? new PagedResult<CompetencyDto>([], 0, page, pageSize);
    }

    public async Task<CompetencyDto> CreateAsync(CreateCompetencyDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/competencies", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CompetencyDto>())!;
    }

    public async Task UpdateAsync(UpdateCompetencyDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/competencies/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/competencies/{id}");
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
