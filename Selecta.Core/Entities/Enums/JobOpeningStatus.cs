namespace Selecta.Core.Entities.Enums;

/// <summary>
/// Corresponde ao enum "StatusVaga" original. Os valores numéricos têm de se
/// manter EXATAMENTE iguais — é assim que já estão gravados na coluna
/// "Status" da tabela "Vagas".
/// </summary>
public enum JobOpeningStatus
{
    New = 0,
    InProgress = 1,
    InReplacement = 2,
    Finished = 3,
    Cancelled = 4,
}
