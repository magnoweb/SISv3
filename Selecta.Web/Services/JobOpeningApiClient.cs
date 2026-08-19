using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class JobOpeningApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<JobOpeningDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<JobOpeningDto>>("api/jobopenings") ?? [];
    }

    public async Task<PagedResult<JobOpeningDto>> GetPagedAsync(int page, int pageSize, bool activeOnly = false, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<JobOpeningDto>>($"api/jobopenings/paged?{query}&activeOnly={activeOnly}")
            ?? new PagedResult<JobOpeningDto>([], 0, page, pageSize);
    }

    public async Task<JobOpeningDto> CreateAsync(CreateJobOpeningDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/jobopenings", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<JobOpeningDto>())!;
    }

    public async Task UpdateAsync(UpdateJobOpeningDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/jobopenings/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>Lança com a mensagem de negócio (ex.: transição de status inválida) quando a Api devolve 400.</summary>
    public async Task ChangeStatusAsync(ChangeJobOpeningStatusDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PatchAsJsonAsync($"api/jobopenings/{dto.Id}/status", dto);

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
        var response = await http.DeleteAsync($"api/jobopenings/{id}");
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
