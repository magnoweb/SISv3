using Selecta.Core.Dtos;
using Selecta.Core.Entities.Schedule;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ScheduleBlockService(IScheduleBlockRepository repository) : IScheduleBlockService
{
    public async Task<IEnumerable<ScheduleBlockDto>> GetAllAsync(CancellationToken ct = default)
    {
        var blocks = await repository.GetAllAsync(ct);
        return blocks.Select(ToDto);
    }

    public async Task<PagedResult<ScheduleBlockDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ScheduleBlockDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ScheduleBlockDto> CreateAsync(CreateScheduleBlockDto dto, CancellationToken ct = default)
    {
        var block = new ScheduleBlock { Origin = dto.Origin, Date = dto.Date, Time = dto.Time, UserId = dto.UserId };
        await repository.AddAsync(block, ct);
        await repository.SaveChangesAsync(ct);

        var created = await repository.GetByIdAsync(block.Id, ct) ?? block;
        return ToDto(created);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var block = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"ScheduleBlock {id} not found.");

        repository.Remove(block);
        await repository.SaveChangesAsync(ct);
    }

    private static ScheduleBlockDto ToDto(ScheduleBlock b) =>
        new(b.Id, b.Origin, b.Date, b.Time, b.UserId, b.User?.Name ?? "(unknown)", b.CreatedAt);
}
