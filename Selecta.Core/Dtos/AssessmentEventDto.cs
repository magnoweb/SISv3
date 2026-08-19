using Selecta.Core.Entities.Enums;

namespace Selecta.Core.Dtos;

public record AssessmentEventDto(
    Guid Id,
    Guid CandidateId, string CandidateName,
    Guid JobTitleId, string JobTitleName,
    Guid? ContactId, string? ContactName,
    DateTime Date,
    EducationLevel EducationLevel,
    string? Education,
    bool EducationCompleted,
    MaritalStatus MaritalStatus,
    string? DriverLicenseNumber,
    DriverLicenseCategory DriverLicenseCategory,
    int? NumberOfChildren,
    string? Address, string? AddressComplement, string? Neighborhood, string? CityName, string? State, string? PostalCode,
    string? Phone1, string? Phone2, string? Email,
    Guid? CityId,
    AssessmentResult Result,
    Guid? EvaluationResultId, string? EvaluationResultName,
    AssessmentStatus Status,
    AssessmentPurpose Purpose,
    DateTime CreatedAt,
    DateTime? CompletedAt);

public record CreateAssessmentEventDto(
    Guid CandidateId, Guid JobTitleId, Guid? ContactId, DateTime Date,
    EducationLevel EducationLevel, string? Education, bool EducationCompleted, MaritalStatus MaritalStatus,
    string? DriverLicenseNumber, DriverLicenseCategory DriverLicenseCategory, int? NumberOfChildren,
    string? Address, string? AddressComplement, string? Neighborhood, string? CityName, string? State, string? PostalCode,
    string? Phone1, string? Phone2, string? Email, Guid? CityId, AssessmentPurpose Purpose);

public record UpdateAssessmentEventDto(
    Guid Id, Guid CandidateId, Guid JobTitleId, Guid? ContactId, DateTime Date,
    EducationLevel EducationLevel, string? Education, bool EducationCompleted, MaritalStatus MaritalStatus,
    string? DriverLicenseNumber, DriverLicenseCategory DriverLicenseCategory, int? NumberOfChildren,
    string? Address, string? AddressComplement, string? Neighborhood, string? CityName, string? State, string? PostalCode,
    string? Phone1, string? Phone2, string? Email, Guid? CityId,
    AssessmentResult Result, Guid? EvaluationResultId, AssessmentStatus Status, AssessmentPurpose Purpose, DateTime? CompletedAt);
