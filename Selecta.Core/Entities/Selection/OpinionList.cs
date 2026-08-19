using Selecta.Core.Entities.Common;
using Selecta.Core.Entities.Security;

namespace Selecta.Core.Entities.Selection;

/// <summary>
/// Agrupa vários AssessmentEvent de uma mesma empresa (via Contact) para
/// enviar um parecer/veredito consolidado — "Lista de Parecer" na v2, com
/// opção de notificar o contato por email. Corresponde a um registo da
/// tabela "ListasParecer" já existente.
/// </summary>
public class OpinionList : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ContactId { get; set; }
    public Contact? Contact { get; set; }

    /// <summary>Corresponde a "ResponsavelId".</summary>
    public Guid ResponsibleId { get; set; }
    public User? Responsible { get; set; }

    /// <summary>
    /// Código gerado automaticamente na criação ("yyyyMMdd_HHmmss" — coluna
    /// tem 20 caracteres, exatamente esse formato, confirmado nas capturas
    /// de tela da v2). Corresponde a "Nome".
    /// </summary>
    public string Code { get; set; } = string.Empty;

    public DateTime Date { get; set; }
    public string? Notes { get; set; }

    /// <summary>Quem criou. Corresponde a "UsuarioId".</summary>
    public Guid CreatedById { get; set; }
    public User? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
