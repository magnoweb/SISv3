using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga AvaliacaoResultadoConfig (EF6).</summary>
public class EvaluationResultConfiguration : IEntityTypeConfiguration<EvaluationResult>
{
    public void Configure(EntityTypeBuilder<EvaluationResult> builder)
    {
        builder.ToTable("AvaliacaoResultados");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id).HasColumnName("AvaliacaoResultadoId");
        builder.Property(r => r.Name).HasColumnName("Nome").IsRequired();
        builder.Property(r => r.Value).HasColumnName("Valor").IsRequired();
        builder.Property(r => r.CssClass).HasColumnName("Class").IsRequired().HasMaxLength(50);
    }
}
