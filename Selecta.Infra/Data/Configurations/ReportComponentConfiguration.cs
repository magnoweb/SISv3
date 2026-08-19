using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga LaudoComponenteConfig (EF6).</summary>
public class ReportComponentConfiguration : IEntityTypeConfiguration<ReportComponent>
{
    public void Configure(EntityTypeBuilder<ReportComponent> builder)
    {
        builder.ToTable("LaudoComponentes");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("LaudoComponenteId");
        builder.Property(c => c.ComponentType).HasColumnName("TipoComponente").IsRequired();
        builder.Property(c => c.Name).HasColumnName("Nome").IsRequired().HasMaxLength(100);
        builder.Property(c => c.Tag).HasColumnName("Tag").HasMaxLength(60);
        builder.Property(c => c.FileName).HasColumnName("Arquivo").HasMaxLength(60);
        builder.Property(c => c.Content).HasColumnName("Conteudo");
        builder.Property(c => c.Active).HasColumnName("Ativo").IsRequired();
    }
}
