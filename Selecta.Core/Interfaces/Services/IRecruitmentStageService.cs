using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface IRecruitmentStageService
{
    Task<IEnumerable<RecruitmentStageDto>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Página 1-based — usada pela tela de listagem.</summary>
    Task<PagedResult<RecruitmentStageDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);

    Task<RecruitmentStageDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<RecruitmentStageDto> CreateAsync(CreateRecruitmentStageDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateRecruitmentStageDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
