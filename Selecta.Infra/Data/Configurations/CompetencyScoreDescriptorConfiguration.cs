using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga ScoreCompetenciaDescritivoConfig (EF6).</summary>
public class CompetencyScoreDescriptorConfiguration : IEntityTypeConfiguration<CompetencyScoreDescriptor>
{
    public void Configure(EntityTypeBuilder<CompetencyScoreDescriptor> builder)
    {
        builder.ToTable("ScoreCompetenciaDescritivos");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("ScoreCompetenciaDescritivoId");
        builder.Property(d => d.CompetencyDescriptorId).HasColumnName("CompetenciaDescritivoId").IsRequired();
        builder.Property(d => d.CompetencyScoreId).HasColumnName("ScoreCompetenciaId").IsRequired();
        builder.Property(d => d.Descriptive).HasColumnName("Descritivo").IsRequired().HasMaxLength(5000);
        builder.Property(d => d.CreatedAt).HasColumnName("DataInclusao").IsRequired();
        builder.Property(d => d.Active).HasColumnName("Ativo").IsRequired();

        builder.HasOne(d => d.CompetencyDescriptor)
            .WithMany()
            .HasForeignKey(d => d.CompetencyDescriptorId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(d => d.CompetencyScore)
            .WithMany()
            .HasForeignKey(d => d.CompetencyScoreId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
