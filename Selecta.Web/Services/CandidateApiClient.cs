using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class CandidateApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<CandidateDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<CandidateDto>>("api/candidates") ?? [];
    }

    public async Task<PagedResult<CandidateDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<CandidateDto>>($"api/candidates/paged?{query}")
            ?? new PagedResult<CandidateDto>([], 0, page, pageSize);
    }

    /// <summary>Devolve null quando a Api responde 404 (nenhum candidato encontrado).</summary>
    public async Task<CandidateDto?> SearchAsync(string term)
    {
        await AuthenticateAsync();
        var response = await http.GetAsync($"api/candidates/search?term={Uri.EscapeDataString(term)}");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<CandidateDto>()
            : null;
    }

    /// <summary>Lança com a mensagem de negócio (ex.: "CPF inválido") quando a Api devolve 400.</summary>
    public async Task<CandidateDto> CreateAsync(CreateCandidateDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/candidates", dto);
        await EnsureSuccessOrThrowDomainMessageAsync(response);
        return (await response.Content.ReadFromJsonAsync<CandidateDto>())!;
    }

    public async Task UpdateAsync(UpdateCandidateDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/candidates/{dto.Id}", dto);
        await EnsureSuccessOrThrowDomainMessageAsync(response);
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/candidates/{id}");
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
