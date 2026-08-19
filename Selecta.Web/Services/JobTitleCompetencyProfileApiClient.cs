using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class JobTitleCompetencyProfileApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<JobTitleCompetencyProfileDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<JobTitleCompetencyProfileDto>>("api/jobtitlecompetencyprofiles") ?? [];
    }

    public async Task<PagedResult<JobTitleCompetencyProfileDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<JobTitleCompetencyProfileDto>>($"api/jobtitlecompetencyprofiles/paged?{query}")
            ?? new PagedResult<JobTitleCompetencyProfileDto>([], 0, page, pageSize);
    }

    public async Task<JobTitleCompetencyProfileDto> CreateAsync(CreateJobTitleCompetencyProfileDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/jobtitlecompetencyprofiles", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<JobTitleCompetencyProfileDto>())!;
    }

    public async Task UpdateAsync(UpdateJobTitleCompetencyProfileDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/jobtitlecompetencyprofiles/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/jobtitlecompetencyprofiles/{id}");
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
