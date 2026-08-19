using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;
using Selecta.Core.Security;
using Selecta.Core.Services;
using Selecta.Infra.Data;
using Selecta.Infra.Repositories;

namespace Selecta.Infra;

/// <summary>
/// Ponto único de registo de dependências do Core + Infra. Chamado a partir do
/// Program.cs da Selecta.Api. Substitui o antigo projeto Selecta.Infra.Ioc.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddSelectaInfra(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Selecta")
            ?? throw new InvalidOperationException("ConnectionStrings:Selecta não configurada.");

        services.AddDbContext<SelectaDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<IJobOpeningRepository, JobOpeningRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IJobTitleRepository, JobTitleRepository>();
        services.AddScoped<IRecruitmentStageRepository, RecruitmentStageRepository>();
        services.AddScoped<IEvaluationResultRepository, EvaluationResultRepository>();
        services.AddScoped<ICompanyEvaluationResultRepository, CompanyEvaluationResultRepository>();
        services.AddScoped<IServiceOfferingRepository, ServiceOfferingRepository>();
        services.AddScoped<IProspectCompanyRepository, ProspectCompanyRepository>();
        services.AddScoped<IProposalRepository, ProposalRepository>();
        services.AddScoped<IScheduleBlockRepository, ScheduleBlockRepository>();
        services.AddScoped<IProfessionalGroupRepository, ProfessionalGroupRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IReportComponentRepository, ReportComponentRepository>();
        services.AddScoped<IReportTemplateRepository, ReportTemplateRepository>();
        services.AddScoped<IAssessmentEventRepository, AssessmentEventRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<ICollaboratorRepository, CollaboratorRepository>();
        services.AddScoped<IAccessProfileRepository, AccessProfileRepository>();
        services.AddScoped<ICompetencyRepository, CompetencyRepository>();
        services.AddScoped<ICompetencyDescriptorRepository, CompetencyDescriptorRepository>();
        services.AddScoped<ICompetencyScoreRepository, CompetencyScoreRepository>();
        services.AddScoped<ICompetencyScoreDescriptorRepository, CompetencyScoreDescriptorRepository>();
        services.AddScoped<IReportCompetencyRepository, ReportCompetencyRepository>();
        services.AddScoped<IJobTitleCompetencyProfileRepository, JobTitleCompetencyProfileRepository>();
        services.AddScoped<IProfessionalGroupCompetencyProfileRepository, ProfessionalGroupCompetencyProfileRepository>();
        services.AddScoped<ICompetencyProfileLineRepository, CompetencyProfileLineRepository>();
        services.AddScoped<IProductivityEntryRepository, ProductivityEntryRepository>();
        services.AddScoped<IPsychologicalTestRepository, PsychologicalTestRepository>();
        services.AddScoped<IAssessmentEventTestRepository, AssessmentEventTestRepository>();
        services.AddScoped<IReportTemplateComponentRepository, ReportTemplateComponentRepository>();
        services.AddScoped<IOpinionListRepository, OpinionListRepository>();
        services.AddScoped<IOpinionListEntryRepository, OpinionListEntryRepository>();
        services.AddScoped<IRecruitmentScheduleRepository, RecruitmentScheduleRepository>();
        services.AddScoped<ISelectionScheduleRepository, SelectionScheduleRepository>();
        services.AddScoped<IScheduleNoteRepository, ScheduleNoteRepository>();
        services.AddScoped<IDashboardRepository, DashboardRepository>();

        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICandidateService, CandidateService>();
        services.AddScoped<IJobOpeningService, JobOpeningService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IJobTitleService, JobTitleService>();
        services.AddScoped<IRecruitmentStageService, RecruitmentStageService>();
        services.AddScoped<IEvaluationResultService, EvaluationResultService>();
        services.AddScoped<ICompanyEvaluationResultService, CompanyEvaluationResultService>();
        services.AddScoped<IServiceOfferingService, ServiceOfferingService>();
        services.AddScoped<IProspectCompanyService, ProspectCompanyService>();
        services.AddScoped<IProposalService, ProposalService>();
        services.AddScoped<IScheduleBlockService, ScheduleBlockService>();
        services.AddScoped<IProfessionalGroupService, ProfessionalGroupService>();
        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<IReportComponentService, ReportComponentService>();
        services.AddScoped<IReportTemplateService, ReportTemplateService>();
        services.AddScoped<IAssessmentEventService, AssessmentEventService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<ICollaboratorService, CollaboratorService>();
        services.AddScoped<IAccessProfileService, AccessProfileService>();
        services.AddScoped<ICompetencyService, CompetencyService>();
        services.AddScoped<ICompetencyDescriptorService, CompetencyDescriptorService>();
        services.AddScoped<ICompetencyScoreService, CompetencyScoreService>();
        services.AddScoped<ICompetencyScoreDescriptorService, CompetencyScoreDescriptorService>();
        services.AddScoped<IReportCompetencyService, ReportCompetencyService>();
        services.AddScoped<IJobTitleCompetencyProfileService, JobTitleCompetencyProfileService>();
        services.AddScoped<IProfessionalGroupCompetencyProfileService, ProfessionalGroupCompetencyProfileService>();
        services.AddScoped<ICompetencyProfileLineService, CompetencyProfileLineService>();
        services.AddScoped<IProductivityEntryService, ProductivityEntryService>();
        services.AddScoped<IPsychologicalTestService, PsychologicalTestService>();
        services.AddScoped<IAssessmentEventTestService, AssessmentEventTestService>();
        services.AddScoped<IReportTemplateComponentService, ReportTemplateComponentService>();
        services.AddScoped<IOpinionListService, OpinionListService>();
        services.AddScoped<IOpinionListEntryService, OpinionListEntryService>();
        services.AddScoped<IRecruitmentScheduleService, RecruitmentScheduleService>();
        services.AddScoped<ISelectionScheduleService, SelectionScheduleService>();
        services.AddScoped<IScheduleNoteService, ScheduleNoteService>();
        services.AddScoped<IDashboardService, DashboardService>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
