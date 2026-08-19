using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class CompetencyDescriptorApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<CompetencyDescriptorDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<CompetencyDescriptorDto>>("api/competencydescriptors") ?? [];
    }

    public async Task<PagedResult<CompetencyDescriptorDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<CompetencyDescriptorDto>>($"api/competencydescriptors/paged?{query}")
            ?? new PagedResult<CompetencyDescriptorDto>([], 0, page, pageSize);
    }

    public async Task<CompetencyDescriptorDto> CreateAsync(CreateCompetencyDescriptorDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/competencydescriptors", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CompetencyDescriptorDto>())!;
    }

    public async Task UpdateAsync(UpdateCompetencyDescriptorDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/competencydescriptors/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/competencydescriptors/{id}");
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
