using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class OpinionListApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<OpinionListDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<OpinionListDto>>("api/opinionlists") ?? [];
    }

    public async Task<PagedResult<OpinionListDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<OpinionListDto>>($"api/opinionlists/paged?{query}")
            ?? new PagedResult<OpinionListDto>([], 0, page, pageSize);
    }

    public async Task<OpinionListDto> CreateAsync(CreateOpinionListDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/opinionlists", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<OpinionListDto>())!;
    }

    public async Task UpdateAsync(UpdateOpinionListDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/opinionlists/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/opinionlists/{id}");
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
