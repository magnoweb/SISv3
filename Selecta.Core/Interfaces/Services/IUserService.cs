using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IUserService
{
    /// <summary>
    /// Valida as credenciais contra a tabela Usuarios existente.
    /// Devolve null quando o login/password não confere ou o utilizador está inativo.
    /// </summary>
    Task<UserDto?> ValidateCredentialsAsync(string login, string password, CancellationToken ct = default);
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken ct = default);
}
