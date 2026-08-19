using System.Net.Http.Headers;
using System.Net.Http.Json;
using Selecta.Core.Dtos;
using Selecta.Web.Services.Auth;

namespace Selecta.Web.Services;

public class DashboardApiClient(HttpClient http, ITokenStorage tokenStorage)
{
    public async Task<DashboardSummaryDto?> GetSummaryAsync()
    {
        await AuthenticateAsync();
        return await http.GetFromJsonAsync<DashboardSummaryDto>("api/dashboard/summary");
    }

    private async Task AuthenticateAsync()
    {
        var token = await tokenStorage.GetAsync();
        http.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
    }
}
