using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ServiceOfferingApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ServiceOfferingDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ServiceOfferingDto>>("api/serviceofferings") ?? [];
    }

    public async Task<PagedResult<ServiceOfferingDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ServiceOfferingDto>>($"api/serviceofferings/paged?{query}")
            ?? new PagedResult<ServiceOfferingDto>([], 0, page, pageSize);
    }

    public async Task<ServiceOfferingDto> CreateAsync(CreateServiceOfferingDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/serviceofferings", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ServiceOfferingDto>())!;
    }

    public async Task UpdateAsync(UpdateServiceOfferingDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/serviceofferings/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/serviceofferings/{id}");
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
