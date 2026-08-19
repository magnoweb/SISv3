using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Common;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Mapeia a entidade City (nomes em inglês) para a tabela "Cidades" já
/// existente (colunas em português). Equivalente EF Core da antiga
/// Selecta.Infra.Data.EntityConfig.CidadeConfig (EF6).
/// </summary>
public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("Cidades");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("CidadeId");
        builder.Property(c => c.Code).HasColumnName("Codigo").IsRequired();
        builder.Property(c => c.Name).HasColumnName("Nome").IsRequired().HasMaxLength(255);
        builder.Property(c => c.State).HasColumnName("Uf").IsRequired().HasMaxLength(2);
    }
}
