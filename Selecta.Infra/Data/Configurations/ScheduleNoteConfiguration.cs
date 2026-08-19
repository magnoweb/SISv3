using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Schedule;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Mapeia ScheduleNote para a tabela "AgendaObservacoes" já existente.</summary>
public class ScheduleNoteConfiguration : IEntityTypeConfiguration<ScheduleNote>
{
    public void Configure(EntityTypeBuilder<ScheduleNote> builder)
    {
        builder.ToTable("AgendaObservacoes");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id).HasColumnName("AgendaObservacaoId");
        builder.Property(n => n.Origin).HasColumnName("Origem").IsRequired();
        builder.Property(n => n.Date).HasColumnName("Data").IsRequired();
        builder.Property(n => n.Time).HasColumnName("Horario");
        builder.Property(n => n.Description).HasColumnName("Descricao").IsRequired().HasMaxLength(500);
        builder.Property(n => n.CreatedById).HasColumnName("UsuarioId").IsRequired();
        builder.Property(n => n.CreatedAt).HasColumnName("DataInclusao").IsRequired();

        builder.HasOne(n => n.CreatedBy)
            .WithMany()
            .HasForeignKey(n => n.CreatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
