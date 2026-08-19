using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Mapeia ReportCompetency para a tabela "LaudoCompetencias" já existente.
/// Equivalente EF Core da antiga LaudoCompetenciaConfig (EF6). Duas FKs
/// apontam para "ScoreCompetencias" (ProfileScore/Score) — mesmo critério
/// de FKs distintas para a mesma tabela usado em Report (Responsible/
/// Supervisor/...) e JobOpening (Manager/CreatedBy).
/// </summary>
public class ReportCompetencyConfiguration : IEntityTypeConfiguration<ReportCompetency>
{
    public void Configure(EntityTypeBuilder<ReportCompetency> builder)
    {
        builder.ToTable("LaudoCompetencias");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("LaudoCompetenciaId");
        builder.Property(c => c.ReportId).HasColumnName("LaudoId").IsRequired();
        builder.Property(c => c.CompetencyId).HasColumnName("CompetenciaId").IsRequired();
        builder.Property(c => c.CompetencyDescriptorId).HasColumnName("CompetenciaDescritivoId");
        builder.Property(c => c.ProfileScoreId).HasColumnName("ScoreCompetenciaPerfilId");
        builder.Property(c => c.ScoreId).HasColumnName("ScoreCompetenciaId");
        builder.Property(c => c.Percentage).HasColumnName("Percentual");

        builder.HasOne(c => c.Report)
            .WithMany()
            .HasForeignKey(c => c.ReportId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(c => c.Competency)
            .WithMany()
            .HasForeignKey(c => c.CompetencyId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(c => c.CompetencyDescriptor)
            .WithMany()
            .HasForeignKey(c => c.CompetencyDescriptorId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(c => c.ProfileScore)
            .WithMany()
            .HasForeignKey(c => c.ProfileScoreId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(c => c.Score)
            .WithMany()
            .HasForeignKey(c => c.ScoreId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
