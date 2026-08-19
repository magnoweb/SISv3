using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ProfessionalGroupCompetencyProfileApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ProfessionalGroupCompetencyProfileDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ProfessionalGroupCompetencyProfileDto>>("api/professionalgroupcompetencyprofiles") ?? [];
    }

    public async Task<PagedResult<ProfessionalGroupCompetencyProfileDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ProfessionalGroupCompetencyProfileDto>>($"api/professionalgroupcompetencyprofiles/paged?{query}")
            ?? new PagedResult<ProfessionalGroupCompetencyProfileDto>([], 0, page, pageSize);
    }

    public async Task<ProfessionalGroupCompetencyProfileDto> CreateAsync(CreateProfessionalGroupCompetencyProfileDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/professionalgroupcompetencyprofiles", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ProfessionalGroupCompetencyProfileDto>())!;
    }

    public async Task UpdateAsync(UpdateProfessionalGroupCompetencyProfileDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/professionalgroupcompetencyprofiles/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/professionalgroupcompetencyprofiles/{id}");
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
