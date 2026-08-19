using Selecta.Core.Dtos;

namespace Selecta.Core.Interfaces.Services;

public interface ICityService
{
    Task<IEnumerable<CityDto>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Página 1-based — usada pela tela de listagem.</summary>
    Task<PagedResult<CityDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default);

    Task<CityDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<CityDto>> GetByStateAsync(string state, CancellationToken ct = default);
    Task<CityDto> CreateAsync(CreateCityDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateCityDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
