using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class AccessProfileApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<AccessProfileDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<AccessProfileDto>>("api/accessprofiles") ?? [];
    }

    public async Task<PagedResult<AccessProfileDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<AccessProfileDto>>($"api/accessprofiles/paged?{query}")
            ?? new PagedResult<AccessProfileDto>([], 0, page, pageSize);
    }

    public async Task<AccessProfileDto> CreateAsync(CreateAccessProfileDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/accessprofiles", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<AccessProfileDto>())!;
    }

    public async Task UpdateAsync(UpdateAccessProfileDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/accessprofiles/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/accessprofiles/{id}");
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
