using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class CompanyEvaluationResultApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<CompanyEvaluationResultDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<CompanyEvaluationResultDto>>("api/companyevaluationresults") ?? [];
    }

    public async Task<PagedResult<CompanyEvaluationResultDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<CompanyEvaluationResultDto>>($"api/companyevaluationresults/paged?{query}")
            ?? new PagedResult<CompanyEvaluationResultDto>([], 0, page, pageSize);
    }

    public async Task<CompanyEvaluationResultDto> CreateAsync(CreateCompanyEvaluationResultDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/companyevaluationresults", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CompanyEvaluationResultDto>())!;
    }

    public async Task UpdateAsync(UpdateCompanyEvaluationResultDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/companyevaluationresults/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/companyevaluationresults/{id}");
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
