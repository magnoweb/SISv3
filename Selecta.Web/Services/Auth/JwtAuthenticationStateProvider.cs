using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Selecta.Web.Services.Auth;

public class JwtAuthenticationStateProvider(ITokenStorage tokenStorage) : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await tokenStorage.GetAsync();
        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(Anonymous);

        var claims = ReadClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, authenticationType: "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task NotifyLoginAsync(string token)
    {
        await tokenStorage.SaveAsync(token);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(
            new ClaimsPrincipal(new ClaimsIdentity(ReadClaimsFromJwt(token), authenticationType: "jwt")))));
    }

    public async Task NotifyLogoutAsync()
    {
        await tokenStorage.ClearAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(Anonymous)));
    }

    /// <summary>Descodifica o payload do JWT (sem validar assinatura — isso é feito pela Api) só para preencher claims na UI.</summary>
    private static IEnumerable<Claim> ReadClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var padded = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=')
            .Replace('-', '+').Replace('_', '/');
        var bytes = Convert.FromBase64String(padded);
        var json = System.Text.Json.JsonDocument.Parse(bytes);

        foreach (var prop in json.RootElement.EnumerateObject())
        {
            if (prop.Name == "role" && prop.Value.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                foreach (var role in prop.Value.EnumerateArray())
                    yield return new Claim(ClaimTypes.Role, role.GetString() ?? string.Empty);
            }
            else
            {
                yield return new Claim(prop.Name, prop.Value.ToString());
            }
        }
    }
}
