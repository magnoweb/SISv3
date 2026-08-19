namespace Selecta.Core.Dtos;

public record CityDto(Guid Id, int Code, string Name, string State);

public record CreateCityDto(int Code, string Name, string State);

public record UpdateCityDto(Guid Id, int Code, string Name, string State);
