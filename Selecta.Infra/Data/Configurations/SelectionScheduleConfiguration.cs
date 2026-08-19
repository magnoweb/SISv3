using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Schedule;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Mapeia SelectionSchedule para a tabela "AgendaSelecao" já existente.</summary>
public class SelectionScheduleConfiguration : IEntityTypeConfiguration<SelectionSchedule>
{
    public void Configure(EntityTypeBuilder<SelectionSchedule> builder)
    {
        builder.ToTable("AgendaSelecao");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).HasColumnName("AgendaId");
        builder.Property(s => s.AssessmentEventId).HasColumnName("EventoAvaliacaoId");
        builder.Property(s => s.JobTitleId).HasColumnName("CargoId").IsRequired();
        builder.Property(s => s.ContactId).HasColumnName("ContatoId");
        builder.Property(s => s.Origin).HasColumnName("Origem").IsRequired();
        builder.Property(s => s.ClientNotes).HasColumnName("ObservacoesCliente").HasMaxLength(500);
        builder.Property(s => s.Name).HasColumnName("Nome").IsRequired().HasMaxLength(150);
        builder.Property(s => s.Cpf).HasColumnName("Cpf").HasMaxLength(20);
        builder.Property(s => s.Date).HasColumnName("Data").IsRequired();
        builder.Property(s => s.Time).HasColumnName("Horario").IsRequired();
        builder.Property(s => s.Status).HasColumnName("Status").IsRequired();
        builder.Property(s => s.InternalNotes).HasColumnName("ObservacoesInterna").HasMaxLength(500);
        builder.Property(s => s.HasHistory).HasColumnName("TemHistorico").IsRequired();
        builder.Property(s => s.CreatedById).HasColumnName("UsuarioId").IsRequired();
        builder.Property(s => s.CreatedAt).HasColumnName("DataInclusao").IsRequired();

        builder.HasOne(s => s.AssessmentEvent)
            .WithMany()
            .HasForeignKey(s => s.AssessmentEventId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(s => s.JobTitle)
            .WithMany()
            .HasForeignKey(s => s.JobTitleId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(s => s.Contact)
            .WithMany()
            .HasForeignKey(s => s.ContactId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(s => s.CreatedBy)
            .WithMany()
            .HasForeignKey(s => s.CreatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
