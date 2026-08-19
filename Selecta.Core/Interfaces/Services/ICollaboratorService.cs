using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface ICollaboratorService
{
    Task<IEnumerable<CollaboratorDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<CollaboratorDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<CollaboratorDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<CollaboratorDto> CreateAsync(CreateCollaboratorDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateCollaboratorDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
