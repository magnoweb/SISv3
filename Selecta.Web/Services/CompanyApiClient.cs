using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class CompanyApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<CompanyDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<CompanyDto>>("api/companies") ?? [];
    }

    public async Task<PagedResult<CompanyDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<CompanyDto>>($"api/companies/paged?{query}")
            ?? new PagedResult<CompanyDto>([], 0, page, pageSize);
    }

    public async Task<CompanyDto> CreateAsync(CreateCompanyDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/companies", dto);
        await EnsureSuccessOrThrowDomainMessageAsync(response);
        return (await response.Content.ReadFromJsonAsync<CompanyDto>())!;
    }

    public async Task UpdateAsync(UpdateCompanyDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/companies/{dto.Id}", dto);
        await EnsureSuccessOrThrowDomainMessageAsync(response);
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/companies/{id}");
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
