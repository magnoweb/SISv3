using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class PsychologicalTestApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<PsychologicalTestDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<PsychologicalTestDto>>("api/psychologicaltests") ?? [];
    }

    public async Task<PagedResult<PsychologicalTestDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<PsychologicalTestDto>>($"api/psychologicaltests/paged?{query}")
            ?? new PagedResult<PsychologicalTestDto>([], 0, page, pageSize);
    }

    public async Task<PsychologicalTestDto> CreateAsync(CreatePsychologicalTestDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/psychologicaltests", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<PsychologicalTestDto>())!;
    }

    public async Task UpdateAsync(UpdatePsychologicalTestDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/psychologicaltests/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/psychologicaltests/{id}");
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
