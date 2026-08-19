using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ContactApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ContactDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ContactDto>>("api/contacts") ?? [];
    }

    public async Task<PagedResult<ContactDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ContactDto>>($"api/contacts/paged?{query}")
            ?? new PagedResult<ContactDto>([], 0, page, pageSize);
    }

    public async Task<ContactDto> CreateAsync(CreateContactDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/contacts", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ContactDto>())!;
    }

    public async Task UpdateAsync(UpdateContactDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/contacts/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/contacts/{id}");
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
