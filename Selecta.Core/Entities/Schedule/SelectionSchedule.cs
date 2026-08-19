using Selecta.Core.Entities.Common;
using Selecta.Core.Entities.Enums;
using Selecta.Core.Entities.Security;
using Selecta.Core.Entities.Selection;

namespace Selecta.Core.Entities.Schedule;

/// <summary>
/// Um agendamento de seleção (avaliação psicológica) para um cargo — a tela
/// "Agenda Seleção" mostrada nas capturas de tela partilhadas. Corresponde
/// a um registo da tabela "AgendaSelecao" já existente. Ver nota em
/// <see cref="RecruitmentSchedule"/> sobre a ausência de herança na BD real.
/// </summary>
public class SelectionSchedule : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? AssessmentEventId { get; set; }
    public AssessmentEvent? AssessmentEvent { get; set; }

    public Guid JobTitleId { get; set; }
    public JobTitle? JobTitle { get; set; }

    /// <summary>"Solicitante" nas capturas de tela. Corresponde a "ContatoId".</summary>
    public Guid? ContactId { get; set; }
    public Contact? Contact { get; set; }

    public ServiceOrigin Origin { get; set; }

    /// <summary>"Observações do Cliente" nas capturas de tela. Corresponde a "ObservacoesCliente".</summary>
    public string? ClientNotes { get; set; }

    /// <summary>Nome do candidato — texto livre, não FK para Candidate. Corresponde a "Nome".</summary>
    public string Name { get; set; } = string.Empty;

    public string? Cpf { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public ScheduleStatus Status { get; set; } = ScheduleStatus.Pending;

    /// <summary>"Observações" nas capturas de tela. Corresponde a "ObservacoesInterna".</summary>
    public string? InternalNotes { get; set; }

    public bool HasHistory { get; set; }

    public Guid CreatedById { get; set; }
    public User? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
