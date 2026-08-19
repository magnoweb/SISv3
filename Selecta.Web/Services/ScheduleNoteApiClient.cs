using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ScheduleNoteApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ScheduleNoteDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ScheduleNoteDto>>("api/schedulenotes") ?? [];
    }

    public async Task<PagedResult<ScheduleNoteDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ScheduleNoteDto>>($"api/schedulenotes/paged?{query}")
            ?? new PagedResult<ScheduleNoteDto>([], 0, page, pageSize);
    }

    public async Task<ScheduleNoteDto> CreateAsync(CreateScheduleNoteDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/schedulenotes", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ScheduleNoteDto>())!;
    }

    public async Task UpdateAsync(UpdateScheduleNoteDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/schedulenotes/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/schedulenotes/{id}");
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
