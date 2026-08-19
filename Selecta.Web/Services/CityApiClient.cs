using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class CityApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<CityDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<CityDto>>("api/cities") ?? [];
    }

    public async Task<PagedResult<CityDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<CityDto>>($"api/cities/paged?{query}")
            ?? new PagedResult<CityDto>([], 0, page, pageSize);
    }

    public async Task<CityDto> CreateAsync(CreateCityDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/cities", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CityDto>())!;
    }

    public async Task UpdateAsync(UpdateCityDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/cities/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/cities/{id}");
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
