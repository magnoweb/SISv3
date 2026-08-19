using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class CollaboratorApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<CollaboratorDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<CollaboratorDto>>("api/collaborators") ?? [];
    }

    public async Task<PagedResult<CollaboratorDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<CollaboratorDto>>($"api/collaborators/paged?{query}")
            ?? new PagedResult<CollaboratorDto>([], 0, page, pageSize);
    }

    public async Task<CollaboratorDto> CreateAsync(CreateCollaboratorDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/collaborators", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CollaboratorDto>())!;
    }

    public async Task UpdateAsync(UpdateCollaboratorDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/collaborators/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/collaborators/{id}");
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
