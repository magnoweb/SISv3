namespace Selecta.Core.Security;

public interface IPasswordHasher
{
    /// <summary>Confere a password em texto simples contra o hash guardado (legado ou novo).</summary>
    bool Verify(string plainPassword, string storedHash);

    /// <summary>Gera um novo hash (formato atual, não o legado) para uma password.</summary>
    string Hash(string plainPassword);

    /// <summary>Indica se o hash guardado ainda está no formato legado (MD5) e devia ser atualizado.</summary>
    bool IsLegacyHash(string storedHash);
}
