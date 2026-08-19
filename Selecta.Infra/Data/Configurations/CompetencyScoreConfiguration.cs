using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga ScoreCompetenciaConfig (EF6).</summary>
public class CompetencyScoreConfiguration : IEntityTypeConfiguration<CompetencyScore>
{
    public void Configure(EntityTypeBuilder<CompetencyScore> builder)
    {
        builder.ToTable("ScoreCompetencias");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).HasColumnName("ScoreCompetenciaId");
        builder.Property(s => s.Name).HasColumnName("Nome").IsRequired();
        builder.Property(s => s.Acronym).HasColumnName("Sigla").IsRequired().HasMaxLength(2).HasColumnType("char(2)");
        builder.Property(s => s.Color).HasColumnName("Cor").IsRequired().HasMaxLength(7);
        builder.Property(s => s.Value).HasColumnName("Valor").IsRequired();
        builder.Property(s => s.Description).HasColumnName("Descricao").HasMaxLength(500);
        builder.Property(s => s.Active).HasColumnName("Ativo").IsRequired();
    }
}
