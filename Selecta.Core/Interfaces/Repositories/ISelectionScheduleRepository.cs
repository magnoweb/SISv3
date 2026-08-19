using Selecta.Core.Entities.Schedule;

namespace Selecta.Core.Interfaces.Repositories;

public interface ISelectionScheduleRepository : IRepositoryBase<SelectionSchedule>
{
    /// <summary>Usado para calcular SelectionSchedule.HasHistory automaticamente na criação — ver SelectionScheduleService.CreateAsync.</summary>
    Task<bool> HasPriorEntriesAsync(string cpf, CancellationToken ct = default);
}
