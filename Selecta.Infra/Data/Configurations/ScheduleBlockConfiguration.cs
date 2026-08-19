using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Schedule;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga AgendaBloqueioConfig (EF6).</summary>
public class ScheduleBlockConfiguration : IEntityTypeConfiguration<ScheduleBlock>
{
    public void Configure(EntityTypeBuilder<ScheduleBlock> builder)
    {
        builder.ToTable("AgendaBloqueios");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id).HasColumnName("AgendaBloqueioId");
        builder.Property(b => b.Origin).HasColumnName("Origem").IsRequired();
        builder.Property(b => b.Date).HasColumnName("Data").IsRequired();
        builder.Property(b => b.Time).HasColumnName("Horario");
        builder.Property(b => b.UserId).HasColumnName("UsuarioId").IsRequired();
        builder.Property(b => b.CreatedAt).HasColumnName("DataInclusao").IsRequired();

        builder.HasIndex(b => b.Origin);
        builder.HasIndex(b => b.Date);

        builder.HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
