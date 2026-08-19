using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Common;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga ColaboradorConfig (EF6).</summary>
public class CollaboratorConfiguration : IEntityTypeConfiguration<Collaborator>
{
    public void Configure(EntityTypeBuilder<Collaborator> builder)
    {
        builder.ToTable("Colaboradores");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("ColaboradorId");
        builder.Property(c => c.Name).HasColumnName("Nome").IsRequired().HasMaxLength(150);
        builder.Property(c => c.Document).HasColumnName("Documento").HasMaxLength(20);
        builder.Property(c => c.Active).HasColumnName("Ativo").IsRequired();
        builder.Property(c => c.CreatedAt).HasColumnName("DataInclusao").IsRequired();
    }
}
