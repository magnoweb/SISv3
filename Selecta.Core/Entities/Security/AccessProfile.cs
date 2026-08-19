namespace Selecta.Core.Entities.Security;

/// <summary>
/// Catálogo de perfis de acesso (papéis) do sistema — ex.: "Admin",
/// "Recrutador". Corresponde a um registo da tabela "PerfisAcesso" já
/// existente. Não há, nesta fase, uma relação normalizada entre User e
/// AccessProfile — User.Roles continua a ser o texto livre já existente
/// (coluna "Perfis" da tabela Usuarios).
/// </summary>
public class AccessProfile : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
