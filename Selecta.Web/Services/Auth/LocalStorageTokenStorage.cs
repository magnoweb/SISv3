using Blazored.LocalStorage;

namespace Selecta.Web.Services.Auth;

public class LocalStorageTokenStorage(ILocalStorageService localStorage) : ITokenStorage
{
    private const string Key = "selecta.auth.token";

    public Task SaveAsync(string token) => localStorage.SetItemAsStringAsync(Key, token).AsTask();

    public Task<string?> GetAsync() => localStorage.GetItemAsStringAsync(Key).AsTask();

    public Task ClearAsync() => localStorage.RemoveItemAsync(Key).AsTask();
}
