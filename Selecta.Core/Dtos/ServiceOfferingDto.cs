namespace Selecta.Core.Dtos;

public record ServiceOfferingDto(Guid Id, string Name, string? Description, bool Recruitment, bool Selection, bool Proposal, bool Active);

public record CreateServiceOfferingDto(string Name, string? Description, bool Recruitment, bool Selection, bool Proposal);

public record UpdateServiceOfferingDto(Guid Id, string Name, string? Description, bool Recruitment, bool Selection, bool Proposal, bool Active);
