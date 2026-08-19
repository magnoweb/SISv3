using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga AtividadeConfig (EF6). Chave int identity — ver nota em Activity.cs.</summary>
public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("Atividades");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasColumnName("AtividadeId").ValueGeneratedOnAdd();
        builder.Property(a => a.Name).HasColumnName("Nome").IsRequired().HasMaxLength(150);
        builder.Property(a => a.Duration).HasColumnName("Tempo").IsRequired();
        builder.Property(a => a.FlexibleDuration).HasColumnName("TempoFlexivel").IsRequired();
        builder.Property(a => a.Origin).HasColumnName("Origem").IsRequired();
        builder.Property(a => a.System).HasColumnName("Sistema").IsRequired();
        builder.Property(a => a.Active).HasColumnName("Ativo").IsRequired();
    }
}
