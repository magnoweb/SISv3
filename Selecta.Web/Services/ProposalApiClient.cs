using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class ProposalApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<ProposalDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<ProposalDto>>("api/proposals") ?? [];
    }

    public async Task<PagedResult<ProposalDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<ProposalDto>>($"api/proposals/paged?{query}")
            ?? new PagedResult<ProposalDto>([], 0, page, pageSize);
    }

    public async Task<ProposalDto> CreateAsync(CreateProposalDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/proposals", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ProposalDto>())!;
    }

    public async Task UpdateAsync(UpdateProposalDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/proposals/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>Lança com a mensagem de negócio (ex.: motivo de recusa em falta) quando a Api devolve 400.</summary>
    public async Task ChangeStatusAsync(ChangeProposalStatusDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PatchAsJsonAsync($"api/proposals/{dto.Id}/status", dto);

        if (response.IsSuccessStatusCode) return;

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var problem = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            throw new InvalidOperationException(problem?.GetValueOrDefault("message") ?? "Status inválido.");
        }

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/proposals/{id}");
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
