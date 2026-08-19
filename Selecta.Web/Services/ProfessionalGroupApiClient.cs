using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ProfessionalGroupApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ProfessionalGroupDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ProfessionalGroupDto>>("api/professionalgroups") ?? [];
    }

    public async Task<PagedResult<ProfessionalGroupDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ProfessionalGroupDto>>($"api/professionalgroups/paged?{query}")
            ?? new PagedResult<ProfessionalGroupDto>([], 0, page, pageSize);
    }

    public async Task<ProfessionalGroupDto> CreateAsync(CreateProfessionalGroupDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/professionalgroups", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ProfessionalGroupDto>())!;
    }

    public async Task UpdateAsync(UpdateProfessionalGroupDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/professionalgroups/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/professionalgroups/{id}");
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
