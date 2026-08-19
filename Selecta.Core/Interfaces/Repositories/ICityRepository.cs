using Selecta.Core.Entities.Common;

namespace Selecta.Core.Interfaces.Repositories;

public interface ICityRepository : IRepositoryBase<City>
{
    Task<IEnumerable<City>> GetByStateAsync(string state, CancellationToken ct = default);
}
