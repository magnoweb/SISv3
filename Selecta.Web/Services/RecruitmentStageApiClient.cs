using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class RecruitmentStageApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<RecruitmentStageDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<RecruitmentStageDto>>("api/recruitmentstages") ?? [];
    }

    public async Task<PagedResult<RecruitmentStageDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<RecruitmentStageDto>>($"api/recruitmentstages/paged?{query}")
            ?? new PagedResult<RecruitmentStageDto>([], 0, page, pageSize);
    }

    public async Task<RecruitmentStageDto> CreateAsync(CreateRecruitmentStageDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/recruitmentstages", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<RecruitmentStageDto>())!;
    }

    public async Task UpdateAsync(UpdateRecruitmentStageDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/recruitmentstages/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/recruitmentstages/{id}");
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
