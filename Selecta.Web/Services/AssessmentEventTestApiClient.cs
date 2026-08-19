using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class AssessmentEventTestApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<AssessmentEventTestDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<AssessmentEventTestDto>>("api/assessmenteventtests") ?? [];
    }

    public async Task<PagedResult<AssessmentEventTestDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<AssessmentEventTestDto>>($"api/assessmenteventtests/paged?{query}")
            ?? new PagedResult<AssessmentEventTestDto>([], 0, page, pageSize);
    }

    public async Task<AssessmentEventTestDto> CreateAsync(CreateAssessmentEventTestDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/assessmenteventtests", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<AssessmentEventTestDto>())!;
    }

    public async Task UpdateAsync(UpdateAssessmentEventTestDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/assessmenteventtests/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/assessmenteventtests/{id}");
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
