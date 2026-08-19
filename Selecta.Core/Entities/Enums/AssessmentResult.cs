namespace Selecta.Core.Entities.Enums;

/// <summary>
/// Corresponde ao enum "ResultadoAvaliacao" original — valores numéricos
/// preservados (incluindo o salto para 99/100, que já era assim no original).
/// </summary>
public enum AssessmentResult
{
    NoResult = 0,
    Advisable = 1,
    AdvisableWithRestrictions = 2,
    ResultNotRequired = 99,
    NotAdvisable = 100,
}
