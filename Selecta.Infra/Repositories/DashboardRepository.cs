using Dapper;
using Selecta.Core.Dtos;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

/// <summary>
/// Único repositório desta solução que usa Dapper/SQL direto em vez do EF
/// Core — propositadamente, para as contagens e agregados do dashboard (ver
/// nota completa na versão anterior deste ficheiro / README). Expandido
/// para espelhar os widgets do dashboard da v2 (ver capturas de tela
/// partilhadas): contagens de avaliações por dia/mês/ano, distribuição de
/// resultado, gráfico mensal dos últimos 12 meses e aniversariantes do dia.
///
/// "Aniversariantes" são CONTATOS (pessoas nas empresas clientes), não
/// candidatos — confirmado pelo código original partilhado:
/// <c>Contatos.Where(c => c.DiaAniversario == hoje.Day &amp;&amp;
/// c.MesAniversario == hoje.Month &amp;&amp; c.Ativo &amp;&amp;
/// c.ReceberNotificacoes)</c>. Mesma regra reproduzida aqui.
///
/// "Entrevistas" (contagens de Agenda) e os painéis de prévia "Agenda
/// Seleção"/"Agenda Recrutamento" do dashboard original ficam de fora por
/// agora — dependem do módulo de Agenda (AgendaSelecao/AgendaRecrutamento),
/// ainda não portado.
/// </summary>
public class DashboardRepository(ISqlConnectionFactory connectionFactory) : IDashboardRepository
{
    private const string Sql = """
        SELECT COUNT(*) FROM Candidatos;
        SELECT COUNT(*) FROM Empresas WHERE Ativo = 1;
        SELECT COUNT(*) FROM Vagas WHERE Status IN (0, 1, 2);
        SELECT COUNT(*) FROM Propostas WHERE Status = 0;
        SELECT COUNT(*) FROM EventosAvaliacao;
        SELECT COUNT(*) FROM Laudos;
        SELECT COUNT(*) FROM EventosAvaliacao WHERE CAST(Data AS DATE) = CAST(GETDATE() AS DATE);
        SELECT COUNT(*) FROM EventosAvaliacao WHERE YEAR(Data) = YEAR(GETDATE()) AND MONTH(Data) = MONTH(GETDATE());
        SELECT COUNT(*) FROM EventosAvaliacao WHERE YEAR(Data) = YEAR(GETDATE());

        SELECT Resultado, COUNT(*) AS Total
        FROM EventosAvaliacao
        WHERE Resultado IN (1, 2, 100)
        GROUP BY Resultado;

        SELECT YEAR(Data) AS Year, MONTH(Data) AS Month, COUNT(*) AS Count
        FROM EventosAvaliacao
        WHERE Data >= DATEADD(MONTH, -11, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1))
        GROUP BY YEAR(Data), MONTH(Data)
        ORDER BY YEAR(Data), MONTH(Data);

        SELECT con.Nome AS ContactName, emp.NomeFantasia AS CompanyName
        FROM Contatos con
        INNER JOIN Empresas emp ON emp.EmpresaId = con.EmpresaId
        WHERE con.DiaAniversario = DAY(GETDATE())
          AND con.MesAniversario = MONTH(GETDATE())
          AND con.Ativo = 1
          AND con.ReceberNotificacoes = 1
        ORDER BY con.Nome;
        """;

    public async Task<DashboardSummaryDto> GetSummaryAsync(CancellationToken ct = default)
    {
        using var connection = await connectionFactory.CreateOpenConnectionAsync(ct);
        using var multi = await connection.QueryMultipleAsync(new CommandDefinition(Sql, cancellationToken: ct));

        var totalCandidates = await multi.ReadSingleAsync<int>();
        var activeCompanies = await multi.ReadSingleAsync<int>();
        var activeJobOpenings = await multi.ReadSingleAsync<int>();
        var pendingProposals = await multi.ReadSingleAsync<int>();
        var totalAssessmentEvents = await multi.ReadSingleAsync<int>();
        var totalReports = await multi.ReadSingleAsync<int>();
        var assessmentEventsToday = await multi.ReadSingleAsync<int>();
        var assessmentEventsThisMonth = await multi.ReadSingleAsync<int>();
        var assessmentEventsThisYear = await multi.ReadSingleAsync<int>();

        var resultRows = (await multi.ReadAsync<ResultRow>()).ToDictionary(r => r.Resultado, r => r.Total);
        var advisableCount = resultRows.GetValueOrDefault(1);
        var advisableWithRestrictionsCount = resultRows.GetValueOrDefault(2);
        var notAdvisableCount = resultRows.GetValueOrDefault(100);

        var monthlyRows = (await multi.ReadAsync<MonthlyRow>()).ToDictionary(r => (r.Year, r.Month), r => r.Count);
        var last12Months = BuildLast12Months(monthlyRows);

        var birthdayRows = await multi.ReadAsync<BirthdayRow>();
        var birthdaysToday = birthdayRows
            .Select(b => new BirthdayTodayDto(b.ContactName, b.CompanyName))
            .ToList();

        return new DashboardSummaryDto(
            totalCandidates, activeCompanies, activeJobOpenings, pendingProposals, totalAssessmentEvents, totalReports,
            assessmentEventsToday, assessmentEventsThisMonth, assessmentEventsThisYear,
            advisableCount, advisableWithRestrictionsCount, notAdvisableCount,
            last12Months, birthdaysToday);
    }

    private static List<MonthlyAssessmentCountDto> BuildLast12Months(Dictionary<(int Year, int Month), int> counts)
    {
        var today = DateTime.UtcNow;
        var result = new List<MonthlyAssessmentCountDto>();

        for (var i = 11; i >= 0; i--)
        {
            var reference = today.AddMonths(-i);
            var count = counts.GetValueOrDefault((reference.Year, reference.Month));
            result.Add(new MonthlyAssessmentCountDto(reference.Year, reference.Month, count));
        }

        return result;
    }

    private sealed record ResultRow(int Resultado, int Total);
    private sealed record MonthlyRow(int Year, int Month, int Count);
    private sealed record BirthdayRow(string ContactName, string? CompanyName);
}
