using Selecta.Core.Entities.Enums;
using Selecta.Core.Entities.Security;

namespace Selecta.Core.Entities.Schedule;

/// <summary>
/// Bloqueio de agenda — marca uma data (e opcionalmente hora) como
/// indisponível para um utilizador, para um dado tipo de serviço. Corresponde
/// a um registo da tabela "AgendaBloqueios" já existente.
///
/// As entradas de agendamento propriamente ditas (entrevistas de recrutamento/
/// seleção — antigos AgendaRecrutamento/AgendaSelecao) ficam fora do escopo
/// desta 1ª fase: usam herança EF6 (TPT, a partir da classe abstrata Agenda) e
/// dependem de stored procedures (Agenda_Propagate para recorrência,
/// Agenda_Relatorio para relatórios) que exigem uma análise própria antes de
/// portar.
/// </summary>
public class ScheduleBlock : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Corresponde a "Origem".</summary>
    public ServiceOrigin Origin { get; set; }

    public DateTime Date { get; set; }

    /// <summary>Hora específica do bloqueio; null = o dia inteiro. Corresponde a "Horario".</summary>
    public TimeSpan? Time { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
