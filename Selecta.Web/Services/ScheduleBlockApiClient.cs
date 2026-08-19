using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ScheduleBlockApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ScheduleBlockDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ScheduleBlockDto>>("api/scheduleblocks") ?? [];
    }

    public async Task<PagedResult<ScheduleBlockDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ScheduleBlockDto>>($"api/scheduleblocks/paged?{query}")
            ?? new PagedResult<ScheduleBlockDto>([], 0, page, pageSize);
    }

    public async Task<ScheduleBlockDto> CreateAsync(CreateScheduleBlockDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/scheduleblocks", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ScheduleBlockDto>())!;
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/scheduleblocks/{id}");
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
