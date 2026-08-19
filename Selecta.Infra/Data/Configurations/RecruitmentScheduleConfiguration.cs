using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Schedule;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Mapeia RecruitmentSchedule para a tabela "AgendaRecrutamento" já existente.</summary>
public class RecruitmentScheduleConfiguration : IEntityTypeConfiguration<RecruitmentSchedule>
{
    public void Configure(EntityTypeBuilder<RecruitmentSchedule> builder)
    {
        builder.ToTable("AgendaRecrutamento");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).HasColumnName("AgendaId");
        builder.Property(s => s.JobOpeningId).HasColumnName("VagaId").IsRequired();
        builder.Property(s => s.TicketId).HasColumnName("TicketId").IsRequired();
        // Nome da coluna original tem o erro de digitação "ResponavelId" (falta o 'v') — reproduzido aqui de propósito.
        builder.Property(s => s.ResponsibleId).HasColumnName("ResponavelId").IsRequired();
        builder.Property(s => s.ClientInterview).HasColumnName("EntrevistaComCliente").IsRequired();
        builder.Property(s => s.Hired).HasColumnName("Contratado").IsRequired();
        builder.Property(s => s.ScheduleType).HasColumnName("TipoAgendamento").IsRequired();
        builder.Property(s => s.Result).HasColumnName("Resultado").IsRequired();
        builder.Property(s => s.Name).HasColumnName("Nome").IsRequired().HasMaxLength(150);
        builder.Property(s => s.Cpf).HasColumnName("Cpf").IsRequired().HasMaxLength(20);
        builder.Property(s => s.Date).HasColumnName("Data").IsRequired();
        builder.Property(s => s.Time).HasColumnName("Horario").IsRequired();
        builder.Property(s => s.Status).HasColumnName("Status").IsRequired();
        builder.Property(s => s.InternalNotes).HasColumnName("ObservacoesInterna").HasMaxLength(500);
        builder.Property(s => s.HasHistory).HasColumnName("TemHistorico").IsRequired();
        builder.Property(s => s.CreatedById).HasColumnName("UsuarioId").IsRequired();
        builder.Property(s => s.CreatedAt).HasColumnName("DataInclusao").IsRequired();

        builder.HasOne(s => s.JobOpening)
            .WithMany()
            .HasForeignKey(s => s.JobOpeningId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(s => s.Responsible)
            .WithMany()
            .HasForeignKey(s => s.ResponsibleId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(s => s.CreatedBy)
            .WithMany()
            .HasForeignKey(s => s.CreatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
