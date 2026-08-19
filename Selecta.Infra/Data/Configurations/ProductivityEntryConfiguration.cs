using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Mapeia ProductivityEntry para a tabela "Produtividades" já existente.</summary>
public class ProductivityEntryConfiguration : IEntityTypeConfiguration<ProductivityEntry>
{
    public void Configure(EntityTypeBuilder<ProductivityEntry> builder)
    {
        builder.ToTable("Produtividades");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("ProdutividadeId");
        builder.Property(p => p.AssessmentEventId).HasColumnName("EventoAvaliacaoId").IsRequired();
        builder.Property(p => p.ActivityId).HasColumnName("AtividadeId").IsRequired();
        builder.Property(p => p.Date).HasColumnName("Data").IsRequired();
        builder.Property(p => p.Duration).HasColumnName("Tempo").IsRequired();
        builder.Property(p => p.UserId).HasColumnName("UsuarioId").IsRequired();
        builder.Property(p => p.CreatedAt).HasColumnName("DataInclusao").IsRequired();

        builder.HasOne(p => p.AssessmentEvent)
            .WithMany()
            .HasForeignKey(p => p.AssessmentEventId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(p => p.Activity)
            .WithMany()
            .HasForeignKey(p => p.ActivityId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
