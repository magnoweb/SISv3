using Selecta.Core.Entities.Recruitment;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Infra.Data;

namespace Selecta.Infra.Repositories;

public class RecruitmentStageRepository(SelectaDbContext context) : RepositoryBase<RecruitmentStage>(context), IRecruitmentStageRepository
{
    protected override string DefaultOrderBy => "Order";
}
