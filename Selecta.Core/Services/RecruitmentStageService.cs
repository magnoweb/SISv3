using Selecta.Core.Dtos;
using Selecta.Core.Entities.Recruitment;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class RecruitmentStageService(IRecruitmentStageRepository repository) : IRecruitmentStageService
{
    public async Task<IEnumerable<RecruitmentStageDto>> GetAllAsync(CancellationToken ct = default)
    {
        var stages = await repository.GetAllAsync(ct);
        return stages.OrderBy(s => s.Order).Select(ToDto);
    }

    public async Task<PagedResult<RecruitmentStageDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<RecruitmentStageDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<RecruitmentStageDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var stage = await repository.GetByIdAsync(id, ct);
        return stage is null ? null : ToDto(stage);
    }

    public async Task<RecruitmentStageDto> CreateAsync(CreateRecruitmentStageDto dto, CancellationToken ct = default)
    {
        var stage = new RecruitmentStage
        {
            Name = dto.Name,
            Description = dto.Description,
            CssClass = dto.CssClass,
            Order = dto.Order,
        };

        await repository.AddAsync(stage, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(stage);
    }

    public async Task UpdateAsync(UpdateRecruitmentStageDto dto, CancellationToken ct = default)
    {
        var stage = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"RecruitmentStage {dto.Id} not found.");

        stage.Name = dto.Name;
        stage.Description = dto.Description;
        stage.CssClass = dto.CssClass;
        stage.Order = dto.Order;
        stage.Active = dto.Active;

        repository.Update(stage);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var stage = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"RecruitmentStage {id} not found.");

        repository.Remove(stage);
        await repository.SaveChangesAsync(ct);
    }

    private static RecruitmentStageDto ToDto(RecruitmentStage s) =>
        new(s.Id, s.Name, s.Description, s.CssClass, s.Order, s.Active, s.CreatedAt);
}
