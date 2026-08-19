namespace Selecta.Core.Entities;

/// <summary>
/// Toda entidade mapeada tem uma chave — permite que RepositoryBase&lt;T, TKey&gt;
/// pagine/procure de forma genérica e determinística. A maioria das tabelas
/// usa Guid (ver <see cref="IEntity"/>), mas algumas tabelas legadas (ex.:
/// "Atividades", "TipoLaudos") usam int identity — daí o parâmetro TKey.
/// </summary>
public interface IEntity<TKey>
{
    TKey Id { get; }
}

/// <summary>Atalho para o caso mais comum — chave Guid. A maioria das entidades implementa apenas esta.</summary>
public interface IEntity : IEntity<Guid>;
