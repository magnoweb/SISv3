using Selecta.Core.Dtos;
using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class CollaboratorService(ICollaboratorRepository repository) : ICollaboratorService
{
    public async Task<IEnumerable<CollaboratorDto>> GetAllAsync(CancellationToken ct = default)
    {
        var collaborators = await repository.GetAllAsync(ct);
        return collaborators.Select(ToDto);
    }

    public async Task<PagedResult<CollaboratorDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<CollaboratorDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<CollaboratorDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var collaborator = await repository.GetByIdAsync(id, ct);
        return collaborator is null ? null : ToDto(collaborator);
    }

    public async Task<CollaboratorDto> CreateAsync(CreateCollaboratorDto dto, CancellationToken ct = default)
    {
        var collaborator = new Collaborator { Name = dto.Name, Document = dto.Document };
        await repository.AddAsync(collaborator, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(collaborator);
    }

    public async Task UpdateAsync(UpdateCollaboratorDto dto, CancellationToken ct = default)
    {
        var collaborator = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"Collaborator {dto.Id} not found.");

        collaborator.Name = dto.Name;
        collaborator.Document = dto.Document;
        collaborator.Active = dto.Active;

        repository.Update(collaborator);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var collaborator = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Collaborator {id} not found.");

        repository.Remove(collaborator);
        await repository.SaveChangesAsync(ct);
    }

    private static CollaboratorDto ToDto(Collaborator c) => new(c.Id, c.Name, c.Document, c.Active, c.CreatedAt);
}
