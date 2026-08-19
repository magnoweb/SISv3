using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core do mapeamento original de PerfilCompetencia (EF6).</summary>
public class CompetencyProfileLineConfiguration : IEntityTypeConfiguration<CompetencyProfileLine>
{
    public void Configure(EntityTypeBuilder<CompetencyProfileLine> builder)
    {
        builder.ToTable("PerfilCompetencias");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id).HasColumnName("PerfilCompetenciaId");
        builder.Property(l => l.CompetencyProfileId).HasColumnName("PerfilId").IsRequired();
        builder.Property(l => l.CompetencyId).HasColumnName("CompetenciaId").IsRequired();
        builder.Property(l => l.CompetencyScoreId).HasColumnName("ScoreCompetenciaId");

        builder.HasOne(l => l.CompetencyProfile)
            .WithMany()
            .HasForeignKey(l => l.CompetencyProfileId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(l => l.Competency)
            .WithMany()
            .HasForeignKey(l => l.CompetencyId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(l => l.CompetencyScore)
            .WithMany()
            .HasForeignKey(l => l.CompetencyScoreId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
