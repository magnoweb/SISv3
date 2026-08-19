using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga CompetenciaDescritivoConfig (EF6).</summary>
public class CompetencyDescriptorConfiguration : IEntityTypeConfiguration<CompetencyDescriptor>
{
    public void Configure(EntityTypeBuilder<CompetencyDescriptor> builder)
    {
        builder.ToTable("CompetenciaDescritivos");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("CompetenciaDescritivoId");
        builder.Property(d => d.CompetencyId).HasColumnName("CompetenciaId").IsRequired();
        builder.Property(d => d.UserId).HasColumnName("UsuarioId").IsRequired();
        builder.Property(d => d.Name).HasColumnName("Nome").IsRequired().HasMaxLength(20);
        builder.Property(d => d.CreatedAt).HasColumnName("DataInclusao").IsRequired();
        builder.Property(d => d.Active).HasColumnName("Ativo").IsRequired();

        builder.HasOne(d => d.Competency)
            .WithMany()
            .HasForeignKey(d => d.CompetencyId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
