using Selecta.Core.Entities.Security;

namespace Selecta.Core.Interfaces.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<User?> GetByLoginAsync(string login, CancellationToken ct = default);
}
