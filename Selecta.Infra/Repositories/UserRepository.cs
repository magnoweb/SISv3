using Microsoft.EntityFrameworkCore;
using Selecta.Core.Entities.Security;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class UserRepository(SelectaDbContext context) : RepositoryBase<User>(context), IUserRepository
{
    public async Task<User?> GetByLoginAsync(string login, CancellationToken ct = default) =>
        await DbSet.FirstOrDefaultAsync(u => u.Login == login, ct);
}
