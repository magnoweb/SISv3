using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Radzen;
using Selecta.Web;
using Selecta.Web.Services;
using Selecta.Web.Services.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseAddress = builder.Configuration["ApiBaseAddress"]
    ?? throw new InvalidOperationException("ApiBaseAddress não configurada em wwwroot/appsettings.json.");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddScoped<ITokenStorage, LocalStorageTokenStorage>();
builder.Services.AddScoped<AuthApiClient>();
builder.Services.AddScoped<CityApiClient>();
builder.Services.AddScoped<CandidateApiClient>();
builder.Services.AddScoped<JobOpeningApiClient>();
builder.Services.AddScoped<CompanyApiClient>();
builder.Services.AddScoped<ContactApiClient>();
builder.Services.AddScoped<JobTitleApiClient>();
builder.Services.AddScoped<RecruitmentStageApiClient>();
builder.Services.AddScoped<UserApiClient>();
builder.Services.AddScoped<EvaluationResultApiClient>();
builder.Services.AddScoped<CompanyEvaluationResultApiClient>();
builder.Services.AddScoped<ServiceOfferingApiClient>();
builder.Services.AddScoped<ProspectCompanyApiClient>();
builder.Services.AddScoped<ProposalApiClient>();
builder.Services.AddScoped<ScheduleBlockApiClient>();
builder.Services.AddScoped<ProfessionalGroupApiClient>();
builder.Services.AddScoped<ActivityApiClient>();
builder.Services.AddScoped<ReportComponentApiClient>();
builder.Services.AddScoped<ReportTemplateApiClient>();
builder.Services.AddScoped<AssessmentEventApiClient>();
builder.Services.AddScoped<ReportApiClient>();
builder.Services.AddScoped<CollaboratorApiClient>();
builder.Services.AddScoped<AccessProfileApiClient>();
builder.Services.AddScoped<DashboardApiClient>();
builder.Services.AddScoped<CompetencyApiClient>();
builder.Services.AddScoped<CompetencyDescriptorApiClient>();
builder.Services.AddScoped<CompetencyScoreApiClient>();
builder.Services.AddScoped<CompetencyScoreDescriptorApiClient>();
builder.Services.AddScoped<ReportCompetencyApiClient>();
builder.Services.AddScoped<JobTitleCompetencyProfileApiClient>();
builder.Services.AddScoped<ProfessionalGroupCompetencyProfileApiClient>();
builder.Services.AddScoped<CompetencyProfileLineApiClient>();
builder.Services.AddScoped<ProductivityEntryApiClient>();
builder.Services.AddScoped<PsychologicalTestApiClient>();
builder.Services.AddScoped<AssessmentEventTestApiClient>();
builder.Services.AddScoped<ReportTemplateComponentApiClient>();
builder.Services.AddScoped<OpinionListApiClient>();
builder.Services.AddScoped<OpinionListEntryApiClient>();
builder.Services.AddScoped<RecruitmentScheduleApiClient>();
builder.Services.AddScoped<SelectionScheduleApiClient>();
builder.Services.AddScoped<ScheduleNoteApiClient>();

builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
