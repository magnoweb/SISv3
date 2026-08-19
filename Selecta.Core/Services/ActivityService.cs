using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ActivityService(IActivityRepository repository) : IActivityService
{
    public async Task<IEnumerable<ActivityDto>> GetAllAsync(CancellationToken ct = default)
    {
        var activities = await repository.GetAllAsync(ct);
        return activities.Select(ToDto);
    }

    public async Task<PagedResult<ActivityDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ActivityDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ActivityDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var activity = await repository.GetByIdAsync(id, ct);
        return activity is null ? null : ToDto(activity);
    }

    public async Task<ActivityDto> CreateAsync(CreateActivityDto dto, CancellationToken ct = default)
    {
        var activity = new Activity
        {
            Name = dto.Name,
            Duration = dto.Duration,
            FlexibleDuration = dto.FlexibleDuration,
            Origin = dto.Origin,
            System = false,
        };

        await repository.AddAsync(activity, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(activity);
    }

    public async Task UpdateAsync(UpdateActivityDto dto, CancellationToken ct = default)
    {
        var activity = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"Activity {dto.Id} not found.");

        activity.Name = dto.Name;
        activity.Duration = dto.Duration;
        activity.FlexibleDuration = dto.FlexibleDuration;
        activity.Origin = dto.Origin;
        activity.Active = dto.Active;

        repository.Update(activity);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var activity = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Activity {id} not found.");

        repository.Remove(activity);
        await repository.SaveChangesAsync(ct);
    }

    private static ActivityDto ToDto(Activity a) => new(a.Id, a.Name, a.Duration, a.FlexibleDuration, a.Origin, a.System, a.Active);
}
