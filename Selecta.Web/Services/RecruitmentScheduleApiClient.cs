using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class RecruitmentScheduleApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<List<RecruitmentScheduleDto>> GetAllAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<List<RecruitmentScheduleDto>>("api/recruitmentschedules") ?? [];
    }

    public async Task<PagedResult<RecruitmentScheduleDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null)
    {
        await AuthenticateAsync();
        var query = PagedQueryBuilder.Build(page, pageSize, filter, orderBy);
        return await http.GetFromJsonAsync<PagedResult<RecruitmentScheduleDto>>($"api/recruitmentschedules/paged?{query}")
            ?? new PagedResult<RecruitmentScheduleDto>([], 0, page, pageSize);
    }

    public async Task<RecruitmentScheduleDto> CreateAsync(CreateRecruitmentScheduleDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PostAsJsonAsync("api/recruitmentschedules", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<RecruitmentScheduleDto>())!;
    }

    public async Task UpdateAsync(UpdateRecruitmentScheduleDto dto)
    {
        await AuthenticateAsync();
        var response = await http.PutAsJsonAsync($"api/recruitmentschedules/{dto.Id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        await AuthenticateAsync();
        var response = await http.DeleteAsync($"api/recruitmentschedules/{id}");
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
