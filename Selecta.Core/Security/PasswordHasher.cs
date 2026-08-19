using System.Security.Cryptography;
using System.Text;

namespace Selecta.Core.Security;

/// <summary>
/// A solução original (Selecta.Infra.Shared.Helpers.HashPassword) guarda a password
/// como MD5(ASCII) em maiúsculas — algoritmo fraco, mas é o que já está gravado para
/// todos os utilizadores existentes na base de dados. Para não invalidar o login de
/// ninguém no dia da migração, este serviço:
///   1. Continua a saber validar o formato legado (MD5);
///   2. Gera sempre hashes NOVOS num formato mais forte (PBKDF2-SHA256 com salt);
///   3. Expõe IsLegacyHash(...) para a camada de aplicação decidir re-gravar o hash
///      do utilizador (com a password em texto simples que acabou de validar) no
///      novo formato, no momento do login — migração gradual, sem downtime nem
///      necessidade de resetar passwords de toda a gente.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    // A coluna Usuarios.Senha existente na base de dados é NVARCHAR(50) (ver
    // UserConfiguration). Para não exigir uma migração de schema só para o
    // hashing funcionar, o formato novo tem de caber nesses 50 caracteres:
    // "P1$" + salt(8B→12 base64) + "$" + hash(16B→24 base64) = 40 caracteres.
    // Os parâmetros (algoritmo/iterações) ficam fixos por versão ("P1"), em vez
    // de embutidos na string, exatamente para poupar espaço. Se um dia alargares
    // a coluna (recomendado, ex.: NVARCHAR(255)), aumenta SaltSize/HashSize
    // e cria uma versão "P2" — o Verify(...) continua a suportar ambas.
    private const int Iterations = 100_000;
    private const int SaltSize = 8;
    private const int HashSize = 16;
    private const string Version = "P1";

    public bool Verify(string plainPassword, string storedHash)
    {
        if (string.IsNullOrEmpty(storedHash)) return false;

        return IsLegacyHash(storedHash)
            ? string.Equals(LegacyMd5Hash(plainPassword), storedHash, StringComparison.OrdinalIgnoreCase)
            : VerifyNewHash(plainPassword, storedHash);
    }

    public bool IsLegacyHash(string storedHash) => !storedHash.StartsWith(Version + "$", StringComparison.Ordinal);

    public string Hash(string plainPassword)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(plainPassword, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        return $"{Version}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    private static bool VerifyNewHash(string plainPassword, string storedHash)
    {
        var parts = storedHash.Split('$');
        if (parts.Length != 3) return false;

        var salt = Convert.FromBase64String(parts[1]);
        var expectedHash = Convert.FromBase64String(parts[2]);

        var computedHash = Rfc2898DeriveBytes.Pbkdf2(plainPassword, salt, Iterations, HashAlgorithmName.SHA256, expectedHash.Length);
        return CryptographicOperations.FixedTimeEquals(computedHash, expectedHash);
    }

    /// <summary>Réplica exata de Selecta.Infra.Shared.Helpers.HashPassword, para compatibilidade com a BD existente.</summary>
    private static string LegacyMd5Hash(string password)
    {
        var input = Encoding.ASCII.GetBytes(password);
        var hash = MD5.HashData(input);

        var result = new StringBuilder();
        foreach (var b in hash)
            result.Append(b.ToString("X2"));

        return result.ToString();
    }
}
