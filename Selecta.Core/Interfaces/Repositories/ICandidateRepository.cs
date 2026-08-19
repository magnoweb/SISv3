using Selecta.Core.Entities.Common;

namespace Selecta.Core.Interfaces.Repositories;

public interface ICandidateRepository : IRepositoryBase<Candidate>
{
    Task<Candidate?> GetByCpfAsync(string cpf, CancellationToken ct = default);

    /// <summary>
    /// Reproduz a antiga "busca única" do Candidato (ObterPorNomeCpf): se o termo
    /// for um CPF válido procura por CPF exato, caso contrário procura por nome
    /// (contém). É o comportamento da caixa de pesquisa da tela de candidatos.
    /// </summary>
    Task<Candidate?> SearchByNameOrCpfAsync(string term, CancellationToken ct = default);
}
