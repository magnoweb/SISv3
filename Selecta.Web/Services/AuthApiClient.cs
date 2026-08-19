using System.Net.Http.Json;
using Selecta.Core.Dtos;

namespace Selecta.Web.Services;

public class AuthApiClient(HttpClient http)
{
    public async Task<LoginResponseDto?> LoginAsync(string login, string password)
    {
        var response = await http.PostAsJsonAsync("api/auth/login", new LoginRequestDto(login, password));
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<LoginResponseDto>()
            : null;
    }
}
