using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Mapeia ReportTemplate para a tabela "TipoLaudos" já existente. Equivalente
/// EF Core da antiga TipoLaudoConfig (EF6). Duas FKs apontam para "Atividades"
/// (Production/Reading) e duas para "LaudoComponentes" (Header/Footer,
/// opcionais) — todas com <c>DeleteBehavior.Restrict</c>, mesmo critério
/// usado sempre que há mais de uma FK para a mesma tabela (ver JobOpening).
/// </summary>
public class ReportTemplateConfiguration : IEntityTypeConfiguration<ReportTemplate>
{
    public void Configure(EntityTypeBuilder<ReportTemplate> builder)
    {
        builder.ToTable("TipoLaudos");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id).HasColumnName("TipoLaudoId").ValueGeneratedOnAdd();
        builder.Property(t => t.Name).HasColumnName("Nome").IsRequired().HasMaxLength(150);
        builder.Property(t => t.Template).HasColumnName("Modelo").HasMaxLength(5000);
        builder.Property(t => t.ProductionActivityId).HasColumnName("AtividadeProducaoId").IsRequired();
        builder.Property(t => t.ReadingActivityId).HasColumnName("AtividadeLeituraId").IsRequired();
        builder.Property(t => t.HeaderId).HasColumnName("CabecalhoId");
        builder.Property(t => t.FooterId).HasColumnName("RodapeId");
        builder.Property(t => t.AttachmentReport).HasColumnName("LaudoAnexo").IsRequired();
        builder.Property(t => t.UseCompetencies).HasColumnName("UsarCompetencias").IsRequired();
        builder.Property(t => t.Active).HasColumnName("Ativo").IsRequired();

        builder.HasOne(t => t.ProductionActivity)
            .WithMany()
            .HasForeignKey(t => t.ProductionActivityId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(t => t.ReadingActivity)
            .WithMany()
            .HasForeignKey(t => t.ReadingActivityId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(t => t.Header)
            .WithMany()
            .HasForeignKey(t => t.HeaderId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(t => t.Footer)
            .WithMany()
            .HasForeignKey(t => t.FooterId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
