using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Common;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga GrupoProfissionalConfig (EF6).</summary>
public class ProfessionalGroupConfiguration : IEntityTypeConfiguration<ProfessionalGroup>
{
    public void Configure(EntityTypeBuilder<ProfessionalGroup> builder)
    {
        builder.ToTable("GruposProfissional");
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id).HasColumnName("GrupoProfissionalId");
        builder.Property(g => g.Name).HasColumnName("Nome").IsRequired().HasMaxLength(50);
        builder.Property(g => g.Description).HasColumnName("Descricao").HasMaxLength(500);
        builder.Property(g => g.CreatedAt).HasColumnName("DataInclusao").IsRequired();
        builder.Property(g => g.Active).HasColumnName("Ativo").IsRequired();
    }
}
