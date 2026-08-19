using Selecta.Core.Entities.Enums;
using Selecta.Core.Entities.Security;

namespace Selecta.Core.Entities.Schedule;

/// <summary>
/// Uma nota geral sobre um dia/horário da agenda (não ligada a um
/// agendamento específico) — modal "Observações na agenda" nas capturas de
/// tela partilhadas. Corresponde a um registo da tabela "AgendaObservacoes"
/// já existente.
/// </summary>
public class ScheduleNote : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public ServiceOrigin Origin { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan? Time { get; set; }

    /// <summary>Corresponde a "Descricao".</summary>
    public string Description { get; set; } = string.Empty;

    public Guid CreatedById { get; set; }
    public User? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
