using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class JobTitleApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<JobTitleDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<JobTitleDto>>("api/jobtitles") ?? [];
    }

    public async Task<PagedResult<JobTitleDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<JobTitleDto>>($"api/jobtitles/paged?{query}")
            ?? new PagedResult<JobTitleDto>([], 0, page, pageSize);
    }

    public async Task<JobTitleDto> CreateAsync(CreateJobTitleDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/jobtitles", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<JobTitleDto>())!;
    }

    public async Task UpdateAsync(UpdateJobTitleDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/jobtitles/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/jobtitles/{id}");
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
