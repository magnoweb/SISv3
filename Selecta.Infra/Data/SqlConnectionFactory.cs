using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Selecta.Infra.Data;

/// <summary>
/// Abre ligações ADO.NET cruas para uso com Dapper — separado do
/// SelectaDbContext (EF Core) propositadamente. Usado nas poucas consultas
/// de leitura agregada onde vale a pena trocar o overhead de tracking/
/// materialização de entidades do EF Core por SQL direto (ex.: contagens
/// de dashboard). CRUD normal continua a passar pelo EF Core.
/// </summary>
public interface ISqlConnectionFactory
{
    Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken ct = default);
}

public class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
{
    public async Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken ct = default)
    {
        var connectionString = configuration.GetConnectionString("Selecta")
            ?? throw new InvalidOperationException("ConnectionStrings:Selecta não configurada.");

        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(ct);
        return connection;
    }
}
