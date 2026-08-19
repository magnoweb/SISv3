namespace Selecta.Core.Entities.Enums;

/// <summary>
/// Corresponde ao enum "ResuultadoEntrevista" original (sic — erro de
/// digitação no nome da classe original, "Resuultado"; aqui só o nome de
/// classe muda, os valores numéricos são preservados). Note que os valores
/// NÃO batem com AssessmentResult (que usa 1/2/100) — são enums diferentes
/// com semântica parecida mas escala própria, teria sido um erro reutilizar
/// um pelo outro.
/// </summary>
public enum InterviewResult
{
    NoResult = 0,
    Advisable = 1,
    AdvisableWithRestrictions = 2,
    NotAdvisable = 3,
}
