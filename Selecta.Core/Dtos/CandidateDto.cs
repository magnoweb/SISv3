using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record CandidateDto(Guid Id, string Name, Gender Gender, DateTime BirthDate, string Cpf, string IdentityDocument, DateTime CreatedAt);

public record CreateCandidateDto(string Name, Gender Gender, DateTime BirthDate, string Cpf, string IdentityDocument);

public record UpdateCandidateDto(Guid Id, string Name, Gender Gender, DateTime BirthDate, string Cpf, string IdentityDocument);
