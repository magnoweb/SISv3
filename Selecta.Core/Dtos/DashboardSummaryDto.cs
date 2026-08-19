namespace Selecta.Core.Dtos;

public record DashboardSummaryDto(
    int TotalCandidates,
    int ActiveCompanies,
    int ActiveJobOpenings,
    int PendingProposals,
    int TotalAssessmentEvents,
    int TotalReports,
    int AssessmentEventsToday,
    int AssessmentEventsThisMonth,
    int AssessmentEventsThisYear,
    int AdvisableCount,
    int AdvisableWithRestrictionsCount,
    int NotAdvisableCount,
    List<MonthlyAssessmentCountDto> AssessmentEventsLast12Months,
    List<BirthdayTodayDto> BirthdaysToday);

public record MonthlyAssessmentCountDto(int Year, int Month, int Count);

/// <summary>
/// Corresponde ao "Aniversariantes" do dashboard v2 — CONTATOS (pessoas nas
/// empresas clientes), não candidatos, filtrados para o dia de hoje. Mesma
/// regra do original: <c>DiaAniversario == hoje.Day && MesAniversario ==
/// hoje.Month && Ativo && ReceberNotificacoes</c>.
/// </summary>
public record BirthdayTodayDto(string ContactName, string? CompanyName);
