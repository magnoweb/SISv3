using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga ListaParecerEventoConfig (EF6).</summary>
public class OpinionListEntryConfiguration : IEntityTypeConfiguration<OpinionListEntry>
{
    public void Configure(EntityTypeBuilder<OpinionListEntry> builder)
    {
        builder.ToTable("ListaParecerEventos");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("ListaParecerEventoId");
        builder.Property(e => e.OpinionListId).HasColumnName("ListaParecerId").IsRequired();
        builder.Property(e => e.AssessmentEventId).HasColumnName("EventoAvaliacaoId").IsRequired();
        builder.Property(e => e.Result).HasColumnName("Resultado").IsRequired();
        builder.Property(e => e.EvaluationResultId).HasColumnName("AvaliacaoResultadoId");
        builder.Property(e => e.CreatedAt).HasColumnName("DataInclusao").IsRequired();

        builder.HasOne(e => e.OpinionList)
            .WithMany()
            .HasForeignKey(e => e.OpinionListId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(e => e.AssessmentEvent)
            .WithMany()
            .HasForeignKey(e => e.AssessmentEventId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(e => e.EvaluationResult)
            .WithMany()
            .HasForeignKey(e => e.EvaluationResultId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
