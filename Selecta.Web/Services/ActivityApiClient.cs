using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ActivityApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ActivityDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ActivityDto>>("api/activities") ?? [];
    }

    public async Task<PagedResult<ActivityDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ActivityDto>>($"api/activities/paged?{query}")
            ?? new PagedResult<ActivityDto>([], 0, page, pageSize);
    }

    public async Task<ActivityDto> CreateAsync(CreateActivityDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/activities", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ActivityDto>())!;
    }

    public async Task UpdateAsync(UpdateActivityDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/activities/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/activities/{id}");
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
