using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Entities.Common;

/// <summary>Corresponde a um registo da tabela "Contatos" já existente.</summary>
public class Contact : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }

    public string Name { get; set; } = string.Empty;
    public Gender Gender { get; set; }

    /// <summary>Cargo/função do contato na empresa (texto livre, não é o catálogo de JobTitle). Corresponde a "Cargo".</summary>
    public string? Position { get; set; }

    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
    public string Email { get; set; } = string.Empty;
    public int? BirthDay { get; set; }
    public int? BirthMonth { get; set; }
    public string? Notes { get; set; }

    /// <summary>Se deve receber notificações automáticas (ex.: troca de etapa de uma vaga). Corresponde a "ReceberNotificacoes".</summary>
    public bool ReceiveNotifications { get; set; }

    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
