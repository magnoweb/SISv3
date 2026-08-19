using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class SelectionScheduleApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<SelectionScheduleDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<SelectionScheduleDto>>("api/selectionschedules") ?? [];
    }

    public async Task<PagedResult<SelectionScheduleDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<SelectionScheduleDto>>($"api/selectionschedules/paged?{query}")
            ?? new PagedResult<SelectionScheduleDto>([], 0, page, pageSize);
    }

    public async Task<SelectionScheduleDto> CreateAsync(CreateSelectionScheduleDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/selectionschedules", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<SelectionScheduleDto>())!;
    }

    public async Task UpdateAsync(UpdateSelectionScheduleDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/selectionschedules/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/selectionschedules/{id}");
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
