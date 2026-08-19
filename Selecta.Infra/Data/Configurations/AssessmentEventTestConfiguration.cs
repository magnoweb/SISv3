using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga EventoAvaliacaoTesteConfig (EF6).</summary>
public class AssessmentEventTestConfiguration : IEntityTypeConfiguration<AssessmentEventTest>
{
    public void Configure(EntityTypeBuilder<AssessmentEventTest> builder)
    {
        builder.ToTable("EventoAvaliacaoTestes");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id).HasColumnName("EventoAvaliacaoTesteId");
        builder.Property(t => t.AssessmentEventId).HasColumnName("EventoAvaliacaoId").IsRequired();
        builder.Property(t => t.PsychologicalTestId).HasColumnName("TestePsicologicoId").IsRequired();
        builder.Property(t => t.Percentage).HasColumnName("Percentual");

        builder.HasOne(t => t.AssessmentEvent)
            .WithMany()
            .HasForeignKey(t => t.AssessmentEventId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(t => t.PsychologicalTest)
            .WithMany()
            .HasForeignKey(t => t.PsychologicalTestId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
