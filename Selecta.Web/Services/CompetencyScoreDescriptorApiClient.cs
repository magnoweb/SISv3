using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class CompetencyScoreDescriptorApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<CompetencyScoreDescriptorDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<CompetencyScoreDescriptorDto>>("api/competencyscoredescriptors") ?? [];
    }

    public async Task<PagedResult<CompetencyScoreDescriptorDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<CompetencyScoreDescriptorDto>>($"api/competencyscoredescriptors/paged?{query}")
            ?? new PagedResult<CompetencyScoreDescriptorDto>([], 0, page, pageSize);
    }

    public async Task<CompetencyScoreDescriptorDto> CreateAsync(CreateCompetencyScoreDescriptorDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/competencyscoredescriptors", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CompetencyScoreDescriptorDto>())!;
    }

    public async Task UpdateAsync(UpdateCompetencyScoreDescriptorDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/competencyscoredescriptors/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/competencyscoredescriptors/{id}");
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
