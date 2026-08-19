using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class CompetencyScoreApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<CompetencyScoreDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<CompetencyScoreDto>>("api/competencyscores") ?? [];
    }

    public async Task<PagedResult<CompetencyScoreDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<CompetencyScoreDto>>($"api/competencyscores/paged?{query}")
            ?? new PagedResult<CompetencyScoreDto>([], 0, page, pageSize);
    }

    public async Task<CompetencyScoreDto> CreateAsync(CreateCompetencyScoreDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/competencyscores", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CompetencyScoreDto>())!;
    }

    public async Task UpdateAsync(UpdateCompetencyScoreDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/competencyscores/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/competencyscores/{id}");
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
