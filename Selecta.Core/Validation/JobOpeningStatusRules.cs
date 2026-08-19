using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Validation;

/// <summary>Porta fiel de Vaga.ValidarAtualizacao (Selecta.Domain.Entities.Recruitment.Vaga).</summary>
public static class JobOpeningStatusRules
{
    public static bool CanTransition(JobOpeningStatus from, JobOpeningStatus to) => from switch
    {
        JobOpeningStatus.New => to is JobOpeningStatus.New or JobOpeningStatus.InProgress or JobOpeningStatus.Cancelled,
        JobOpeningStatus.InProgress => to is JobOpeningStatus.InProgress or JobOpeningStatus.Finished or JobOpeningStatus.Cancelled,
        JobOpeningStatus.InReplacement => to is JobOpeningStatus.InReplacement or JobOpeningStatus.Finished or JobOpeningStatus.Cancelled,
        JobOpeningStatus.Finished => to == JobOpeningStatus.InReplacement,
        _ => false, // Cancelled é terminal — nenhuma transição a partir daqui, igual à regra original.
    };
}
