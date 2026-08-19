using Selecta.Core.Entities.Common;
using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Entities.Selection;

/// <summary>
/// "Hub" de uma avaliação — dados do candidato/cargo/contato no momento da
/// avaliação, resultado e status. Corresponde a um registo da tabela
/// "EventosAvaliacao" já existente.
///
/// A relação com Laudo (o original tinha EventoAvaliacao.LaudoId/Laudo,
/// mapeada como 1:1 com chave partilhada a partir do lado de Laudo) fica de
/// fora — Laudo ainda não foi portado. Fora do escopo também: as
/// sub-coleções Produtividades, Testes (EventoAvaliacaoTeste) e Anexos
/// (EventoAvaliacaoAnexo).
/// </summary>
public class AssessmentEvent : Selecta.Core.Entities.IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CandidateId { get; set; }
    public Candidate? Candidate { get; set; }

    /// <summary>Cargo avaliado. Corresponde a "CargoId".</summary>
    public Guid JobTitleId { get; set; }
    public JobTitle? JobTitle { get; set; }

    public Guid? ContactId { get; set; }
    public Contact? Contact { get; set; }

    public DateTime Date { get; set; }

    public EducationLevel EducationLevel { get; set; }

    /// <summary>Curso/área de formação (texto livre). Corresponde a "Formacao".</summary>
    public string? Education { get; set; }

    public bool EducationCompleted { get; set; }
    public MaritalStatus MaritalStatus { get; set; }

    /// <summary>Corresponde a "NumeroHabilitacao".</summary>
    public string? DriverLicenseNumber { get; set; }
    public DriverLicenseCategory DriverLicenseCategory { get; set; }

    public int? NumberOfChildren { get; set; }

    public string? Address { get; set; }
    public string? AddressComplement { get; set; }
    public string? Neighborhood { get; set; }

    /// <summary>Texto livre legado (mesmo padrão dual de Company.CityName). Corresponde a "Cidade".</summary>
    public string? CityName { get; set; }

    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
    public string? Email { get; set; }

    /// <summary>FK normalizada opcional. Corresponde a "CidadeId"/"CidadeEntity".</summary>
    public Guid? CityId { get; set; }
    public City? City { get; set; }

    public AssessmentResult Result { get; set; } = AssessmentResult.NoResult;

    public Guid? EvaluationResultId { get; set; }
    public EvaluationResult? EvaluationResult { get; set; }

    public AssessmentStatus Status { get; set; } = AssessmentStatus.NotStarted;
    public AssessmentPurpose Purpose { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
