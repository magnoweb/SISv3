using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class CompetencyProfileLineApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<CompetencyProfileLineDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<CompetencyProfileLineDto>>("api/competencyprofilelines") ?? [];
    }

    public async Task<PagedResult<CompetencyProfileLineDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<CompetencyProfileLineDto>>($"api/competencyprofilelines/paged?{query}")
            ?? new PagedResult<CompetencyProfileLineDto>([], 0, page, pageSize);
    }

    public async Task<CompetencyProfileLineDto> CreateAsync(CreateCompetencyProfileLineDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/competencyprofilelines", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CompetencyProfileLineDto>())!;
    }

    public async Task UpdateAsync(UpdateCompetencyProfileLineDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/competencyprofilelines/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/competencyprofilelines/{id}");
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
