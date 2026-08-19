using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ProductivityEntryApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ProductivityEntryDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ProductivityEntryDto>>("api/productivityentries") ?? [];
    }

    public async Task<PagedResult<ProductivityEntryDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ProductivityEntryDto>>($"api/productivityentries/paged?{query}")
            ?? new PagedResult<ProductivityEntryDto>([], 0, page, pageSize);
    }

    public async Task<ProductivityEntryDto> CreateAsync(CreateProductivityEntryDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/productivityentries", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ProductivityEntryDto>())!;
    }

    public async Task UpdateAsync(UpdateProductivityEntryDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/productivityentries/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/productivityentries/{id}");
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
