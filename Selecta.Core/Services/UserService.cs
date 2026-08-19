using Selecta.Core.Dtos;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;
using Selecta.Core.Security;

namespace Selecta.Core.Services;

public class UserService(IUserRepository repository, IPasswordHasher hasher) : IUserService
{
    public async Task<UserDto?> ValidateCredentialsAsync(string login, string password, CancellationToken ct = default)
    {
        var user = await repository.GetByLoginAsync(login, ct);
        if (user is null || !user.Active) return null;
        if (!hasher.Verify(password, user.PasswordHash)) return null;

        // Migração gradual: se o hash guardado ainda for o formato legado (MD5),
        // aproveita que temos a password em texto simples neste momento (acabou
        // de ser validada) para a regravar já no formato forte.
        if (hasher.IsLegacyHash(user.PasswordHash))
        {
            user.PasswordHash = hasher.Hash(password);
            repository.Update(user);
            await repository.SaveChangesAsync(ct);
        }

        return ToDto(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken ct = default)
    {
        var users = await repository.GetAllAsync(ct);
        return users.Select(ToDto);
    }

    private static UserDto ToDto(Entities.Security.User u) =>
        new(u.Id, u.Name, u.Email, u.Login, u.IsSystemAdmin, u.Roles);
}
