using Selecta.Core.Dtos;
using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class CityService(ICityRepository repository) : ICityService
{
    public async Task<IEnumerable<CityDto>> GetAllAsync(CancellationToken ct = default)
    {
        var cities = await repository.GetAllAsync(ct);
        return cities.Select(ToDto);
    }

    public async Task<PagedResult<CityDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<CityDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<CityDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var city = await repository.GetByIdAsync(id, ct);
        return city is null ? null : ToDto(city);
    }

    public async Task<IEnumerable<CityDto>> GetByStateAsync(string state, CancellationToken ct = default)
    {
        var cities = await repository.GetByStateAsync(state, ct);
        return cities.Select(ToDto);
    }

    public async Task<CityDto> CreateAsync(CreateCityDto dto, CancellationToken ct = default)
    {
        var city = new City { Code = dto.Code, Name = dto.Name, State = dto.State };
        await repository.AddAsync(city, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(city);
    }

    public async Task UpdateAsync(UpdateCityDto dto, CancellationToken ct = default)
    {
        var city = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"City {dto.Id} not found.");

        city.Code = dto.Code;
        city.Name = dto.Name;
        city.State = dto.State;

        repository.Update(city);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var city = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"City {id} not found.");

        repository.Remove(city);
        await repository.SaveChangesAsync(ct);
    }

    private static CityDto ToDto(City c) => new(c.Id, c.Code, c.Name, c.State);
}
