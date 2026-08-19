using Selecta.Core.Entities.Schedule;

namespace Selecta.Core.Interfaces.Repositories;

public interface IRecruitmentScheduleRepository : IRepositoryBase<RecruitmentSchedule>
{
    /// <summary>Usado para calcular RecruitmentSchedule.HasHistory automaticamente na criação — ver RecruitmentScheduleService.CreateAsync.</summary>
    Task<bool> HasPriorEntriesAsync(string cpf, CancellationToken ct = default);
}
