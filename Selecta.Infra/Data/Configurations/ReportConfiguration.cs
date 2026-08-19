using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Mapeia Report para a tabela "Laudos" já existente. Equivalente EF Core da
/// antiga LaudoConfig (EF6). FK real para AssessmentEvent via coluna própria
/// "EventoAvaliacaoId" — confirmado contra o schema real da BD (não é 1:1
/// de chave partilhada, correção feita depois de ver o script de criação).
/// </summary>
public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable("Laudos");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id).HasColumnName("LaudoId");
        builder.Property(r => r.AssessmentEventId).HasColumnName("EventoAvaliacaoId").IsRequired();
        builder.Property(r => r.ReportTemplateId).HasColumnName("TipoLaudoId").IsRequired();
        builder.Property(r => r.Descriptive).HasColumnName("Descritivo");
        builder.Property(r => r.FileName).HasColumnName("Arquivo").HasMaxLength(100);
        builder.Property(r => r.FileCreatedAt).HasColumnName("ArquivoDataInclusao");
        builder.Property(r => r.ResponsibleId).HasColumnName("ResponsavelId").IsRequired();
        builder.Property(r => r.SupervisorId).HasColumnName("SupervisorId");
        builder.Property(r => r.ResponsibleSignatureId).HasColumnName("ResponsavelAssinaturaId");
        builder.Property(r => r.SupervisorSignatureId).HasColumnName("SupervisorAssinaturaId");
        builder.Property(r => r.Utilization).HasColumnName("Aproveitamento");
        builder.Property(r => r.Average).HasColumnName("Media");
        builder.Property(r => r.UpdatedById).HasColumnName("AtualizacaoUsuarioId");
        builder.Property(r => r.UpdatedAt).HasColumnName("DataAtualizacao");
        builder.Property(r => r.CreatedAt).HasColumnName("DataInclusao").IsRequired();

        // FK normal para AssessmentEvent (coluna própria "EventoAvaliacaoId" — ver nota em Report.cs).
        builder.HasOne(r => r.AssessmentEvent)
            .WithMany()
            .HasForeignKey(r => r.AssessmentEventId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(r => r.ReportTemplate)
            .WithMany()
            .HasForeignKey(r => r.ReportTemplateId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(r => r.Responsible)
            .WithMany()
            .HasForeignKey(r => r.ResponsibleId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(r => r.Supervisor)
            .WithMany()
            .HasForeignKey(r => r.SupervisorId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(r => r.ResponsibleSignature)
            .WithMany()
            .HasForeignKey(r => r.ResponsibleSignatureId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(r => r.SupervisorSignature)
            .WithMany()
            .HasForeignKey(r => r.SupervisorSignatureId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(r => r.UpdatedBy)
            .WithMany()
            .HasForeignKey(r => r.UpdatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
