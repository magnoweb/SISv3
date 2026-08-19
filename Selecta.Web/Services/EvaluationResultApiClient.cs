using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class EvaluationResultApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<EvaluationResultDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<EvaluationResultDto>>("api/evaluationresults") ?? [];
    }

    public async Task<PagedResult<EvaluationResultDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<EvaluationResultDto>>($"api/evaluationresults/paged?{query}")
            ?? new PagedResult<EvaluationResultDto>([], 0, page, pageSize);
    }

    public async Task<EvaluationResultDto> CreateAsync(CreateEvaluationResultDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/evaluationresults", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<EvaluationResultDto>())!;
    }

    public async Task UpdateAsync(UpdateEvaluationResultDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/evaluationresults/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/evaluationresults/{id}");
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
