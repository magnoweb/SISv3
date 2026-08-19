using Selecta.Core.Dtos;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class DashboardService(IDashboardRepository repository) : IDashboardService
{
    public Task<DashboardSummaryDto> GetSummaryAsync(CancellationToken ct = default) =>
        repository.GetSummaryAsync(ct);
}
