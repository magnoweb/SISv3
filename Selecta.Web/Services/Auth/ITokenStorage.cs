namespace Selecta.Web.Services.Auth;

public interface ITokenStorage
{
    Task SaveAsync(string token);
    Task<string?> GetAsync();
    Task ClearAsync();
}
