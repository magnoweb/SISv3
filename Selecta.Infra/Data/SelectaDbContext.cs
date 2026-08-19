using Microsoft.EntityFrameworkCore;
using Selecta.Core.Entities.Administrative;
using Selecta.Core.Entities.Common;
using Selecta.Core.Entities.Recruitment;
using Selecta.Core.Entities.Schedule;
using Selecta.Core.Entities.Security;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data;

/// <summary>
/// DbContext ligado à MESMA base de dados SQL Server da solução Selecta original
/// (schema "selecta_SIS" / "SIS_v2", consoante o ambiente). Nenhuma migration de
/// schema é gerada a partir daqui para as tabelas já existentes — o mapeamento em
/// Data/Configurations apenas descreve o que já está na base de dados (nomes de
/// tabela/coluna em português, entidades em inglês).
///
/// Para tabelas novas, exclusivas deste projeto, podes normalmente usar migrations
/// do EF Core (dotnet ef migrations add ...).
/// </summary>
public class SelectaDbContext(DbContextOptions<SelectaDbContext> options) : DbContext(options)
{
    public DbSet<City> Cities => Set<City>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<JobOpening> JobOpenings => Set<JobOpening>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<JobTitle> JobTitles => Set<JobTitle>();
    public DbSet<RecruitmentStage> RecruitmentStages => Set<RecruitmentStage>();
    public DbSet<EvaluationResult> EvaluationResults => Set<EvaluationResult>();
    public DbSet<CompanyEvaluationResult> CompanyEvaluationResults => Set<CompanyEvaluationResult>();
    public DbSet<ServiceOffering> ServiceOfferings => Set<ServiceOffering>();
    public DbSet<ProspectCompany> ProspectCompanies => Set<ProspectCompany>();
    public DbSet<Proposal> Proposals => Set<Proposal>();
    public DbSet<ScheduleBlock> ScheduleBlocks => Set<ScheduleBlock>();
    public DbSet<ProfessionalGroup> ProfessionalGroups => Set<ProfessionalGroup>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<ReportComponent> ReportComponents => Set<ReportComponent>();
    public DbSet<ReportTemplate> ReportTemplates => Set<ReportTemplate>();
    public DbSet<AssessmentEvent> AssessmentEvents => Set<AssessmentEvent>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<Collaborator> Collaborators => Set<Collaborator>();
    public DbSet<AccessProfile> AccessProfiles => Set<AccessProfile>();
    public DbSet<Competency> Competencies => Set<Competency>();
    public DbSet<CompetencyDescriptor> CompetencyDescriptors => Set<CompetencyDescriptor>();
    public DbSet<CompetencyScore> CompetencyScores => Set<CompetencyScore>();
    public DbSet<CompetencyScoreDescriptor> CompetencyScoreDescriptors => Set<CompetencyScoreDescriptor>();
    public DbSet<ReportCompetency> ReportCompetencies => Set<ReportCompetency>();
    public DbSet<CompetencyProfile> CompetencyProfiles => Set<CompetencyProfile>();
    public DbSet<JobTitleCompetencyProfile> JobTitleCompetencyProfiles => Set<JobTitleCompetencyProfile>();
    public DbSet<ProfessionalGroupCompetencyProfile> ProfessionalGroupCompetencyProfiles => Set<ProfessionalGroupCompetencyProfile>();
    public DbSet<CompetencyProfileLine> CompetencyProfileLines => Set<CompetencyProfileLine>();
    public DbSet<ProductivityEntry> ProductivityEntries => Set<ProductivityEntry>();
    public DbSet<PsychologicalTest> PsychologicalTests => Set<PsychologicalTest>();
    public DbSet<AssessmentEventTest> AssessmentEventTests => Set<AssessmentEventTest>();
    public DbSet<ReportTemplateComponent> ReportTemplateComponents => Set<ReportTemplateComponent>();
    public DbSet<OpinionList> OpinionLists => Set<OpinionList>();
    public DbSet<OpinionListEntry> OpinionListEntries => Set<OpinionListEntry>();
    public DbSet<RecruitmentSchedule> RecruitmentSchedules => Set<RecruitmentSchedule>();
    public DbSet<SelectionSchedule> SelectionSchedules => Set<SelectionSchedule>();
    public DbSet<ScheduleNote> ScheduleNotes => Set<ScheduleNote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SelectaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
