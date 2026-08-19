using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IPsychologicalTestService
{
    Task<IEnumerable<PsychologicalTestDto>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<PsychologicalTestDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);
    Task<PsychologicalTestDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PsychologicalTestDto> CreateAsync(CreatePsychologicalTestDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdatePsychologicalTestDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
