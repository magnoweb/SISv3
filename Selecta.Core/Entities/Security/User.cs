namespace Selecta.Core.Entities.Security;

/// <summary>
/// Corresponde a um subconjunto de colunas da tabela "Usuarios" já existente
/// (o essencial para autenticação e listagem). As restantes colunas originais
/// (EmpresaId, ContatoId, ColaboradorId, Foto, Token, DataValidadeToken,
/// Documento, IdOld, flags de "Funções") continuam na base de dados e podem
/// ser acrescentadas aqui mais tarde — basta adicionar a propriedade em
/// inglês e o respetivo HasColumnName(...) em UserConfiguration, sem quebrar
/// o que já está mapeado.
/// </summary>
public class User : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;

    /// <summary>Hash da password. Ver <see cref="Security.IPasswordHasher"/>. Corresponde à coluna "Senha".</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Lista de perfis separada por vírgula. Corresponde à coluna "Perfis".</summary>
    public string? Roles { get; set; }

    /// <summary>Corresponde à coluna "Ativo".</summary>
    public bool Active { get; set; } = true;

    /// <summary>Corresponde à coluna "SysAdmin".</summary>
    public bool IsSystemAdmin { get; set; }

    /// <summary>Corresponde à coluna "DataInclusao".</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
