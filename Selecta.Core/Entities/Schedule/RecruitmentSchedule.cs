using Selecta.Core.Entities.Enums;
using Selecta.Core.Entities.Recruitment;
using Selecta.Core.Entities.Security;

namespace Selecta.Core.Entities.Schedule;

/// <summary>
/// Um agendamento de recrutamento (entrevista, prova, dinâmica de grupo ou
/// entrevista com gestor) para uma vaga. Corresponde a um registo da tabela
/// "AgendaRecrutamento" já existente.
///
/// Ao contrário do que a hierarquia C# original sugeria (uma classe
/// abstrata "Agenda" com "AgendaRecrutamento"/"AgendaSelecao" como
/// subclasses), o schema real confirma que são duas tabelas totalmente
/// planas e independentes — sem discriminador, sem herança ao nível da BD.
/// Por isso não há uma base C# partilhada aqui: os campos comuns (Nome,
/// Cpf, Data, Horario, Status, ...) estão duplicados nas duas entidades,
/// espelhando fielmente a realidade física.
/// </summary>
public class RecruitmentSchedule : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid JobOpeningId { get; set; }
    public JobOpening? JobOpening { get; set; }

    /// <summary>Sistema de tickets ainda não portado — Guid solto, sem FK. Corresponde a "TicketId".</summary>
    public Guid TicketId { get; set; }

    /// <summary>Corresponde a "ResponavelId" (sic — erro de digitação na coluna original, reproduzido no mapeamento).</summary>
    public Guid ResponsibleId { get; set; }
    public User? Responsible { get; set; }

    public bool ClientInterview { get; set; }
    public bool Hired { get; set; }
    public RecruitmentScheduleType ScheduleType { get; set; }
    public InterviewResult Result { get; set; } = InterviewResult.NoResult;

    /// <summary>Nome do candidato — texto livre, não FK para Candidate. Corresponde a "Nome".</summary>
    public string Name { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public ScheduleStatus Status { get; set; } = ScheduleStatus.Pending;

    /// <summary>Corresponde a "ObservacoesInterna".</summary>
    public string? InternalNotes { get; set; }

    /// <summary>Se este candidato/CPF tem agendamentos anteriores (histórico) — dirige o ícone de histórico na v2. Corresponde a "TemHistorico".</summary>
    public bool HasHistory { get; set; }

    public Guid CreatedById { get; set; }
    public User? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
