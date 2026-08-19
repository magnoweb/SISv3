using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga AvaliacaoResultadoCustomConfig (EF6).</summary>
public class CompanyEvaluationResultConfiguration : IEntityTypeConfiguration<CompanyEvaluationResult>
{
    public void Configure(EntityTypeBuilder<CompanyEvaluationResult> builder)
    {
        builder.ToTable("AvaliacaoResultadosCustom");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id).HasColumnName("AvaliacaoResultadoCustomId");
        builder.Property(r => r.EvaluationResultId).HasColumnName("AvaliacaoResultadoId").IsRequired();
        builder.Property(r => r.CompanyId).HasColumnName("EmpresaId").IsRequired();
        builder.Property(r => r.Name).HasColumnName("Nome").IsRequired();

        builder.HasOne(r => r.EvaluationResult)
            .WithMany()
            .HasForeignKey(r => r.EvaluationResultId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(r => r.Company)
            .WithMany()
            .HasForeignKey(r => r.CompanyId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
