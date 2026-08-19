using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga TipoLaudoComponenteConfig (EF6).</summary>
public class ReportTemplateComponentConfiguration : IEntityTypeConfiguration<ReportTemplateComponent>
{
    public void Configure(EntityTypeBuilder<ReportTemplateComponent> builder)
    {
        builder.ToTable("TipoLaudoComponentes");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("TipoLaudoComponenteId");
        builder.Property(c => c.ReportTemplateId).HasColumnName("TipoLaudoId").IsRequired();
        builder.Property(c => c.ReportComponentId).HasColumnName("LaudoComponenteId").IsRequired();

        builder.HasOne(c => c.ReportTemplate)
            .WithMany()
            .HasForeignKey(c => c.ReportTemplateId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(c => c.ReportComponent)
            .WithMany()
            .HasForeignKey(c => c.ReportComponentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
