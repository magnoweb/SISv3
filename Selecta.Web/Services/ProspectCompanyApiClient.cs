using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ProspectCompanyApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ProspectCompanyDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ProspectCompanyDto>>("api/prospectcompanies") ?? [];
    }

    public async Task<PagedResult<ProspectCompanyDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ProspectCompanyDto>>($"api/prospectcompanies/paged?{query}")
            ?? new PagedResult<ProspectCompanyDto>([], 0, page, pageSize);
    }

    public async Task<ProspectCompanyDto> CreateAsync(CreateProspectCompanyDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/prospectcompanies", dto);
        await EnsureSuccessOrThrowDomainMessageAsync(response);
        return (await response.Content.ReadFromJsonAsync<ProspectCompanyDto>())!;
    }

    public async Task UpdateAsync(UpdateProspectCompanyDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/prospectcompanies/{dto.Id}", dto);
        await EnsureSuccessOrThrowDomainMessageAsync(response);
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/prospectcompanies/{id}");
        response.EnsureSuccessStatusCode();
    }

    private async Task AuthenticateAsync()
    {
        var token = await tokenStorage.GetAsync();
        http.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
    }

    private static async Task EnsureSuccessOrThrowDomainMessageAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var problem = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            throw new InvalidOperationException(problem?.GetValueOrDefault("message") ?? "Dados inválidos.");
        }

        response.EnsureSuccessStatusCode();
    }
}
