using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga CompetenciaConfig (EF6).</summary>
public class CompetencyConfiguration : IEntityTypeConfiguration<Competency>
{
    public void Configure(EntityTypeBuilder<Competency> builder)
    {
        builder.ToTable("Competencias");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("CompetenciaId");
        builder.Property(c => c.Group).HasColumnName("Grupo").IsRequired();
        builder.Property(c => c.Name).HasColumnName("Nome").IsRequired();
        builder.Property(c => c.Description).HasColumnName("Descricao").HasMaxLength(500);
        builder.Property(c => c.CreatedAt).HasColumnName("DataInclusao").IsRequired();
        builder.Property(c => c.Active).HasColumnName("Ativo").IsRequired();
    }
}
