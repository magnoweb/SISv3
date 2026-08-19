using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class AssessmentEventApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<AssessmentEventDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<AssessmentEventDto>>("api/assessmentevents") ?? [];
    }

    public async Task<PagedResult<AssessmentEventDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<AssessmentEventDto>>($"api/assessmentevents/paged?{query}")
            ?? new PagedResult<AssessmentEventDto>([], 0, page, pageSize);
    }

    public async Task<AssessmentEventDto> CreateAsync(CreateAssessmentEventDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/assessmentevents", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<AssessmentEventDto>())!;
    }

    public async Task UpdateAsync(UpdateAssessmentEventDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/assessmentevents/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/assessmentevents/{id}");
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
