using Selecta.Core.Entities.Selection;

namespace Selecta.Core.Interfaces.Repositories;

public interface IReportTemplateComponentRepository : IRepositoryBase<ReportTemplateComponent>
{
    Task<IEnumerable<ReportTemplateComponent>> GetByReportTemplateAsync(int reportTemplateId, CancellationToken ct = default);
}
