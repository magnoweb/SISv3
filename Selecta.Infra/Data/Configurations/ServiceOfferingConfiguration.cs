using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Common;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga ServicoConfig (EF6).</summary>
public class ServiceOfferingConfiguration : IEntityTypeConfiguration<ServiceOffering>
{
    public void Configure(EntityTypeBuilder<ServiceOffering> builder)
    {
        builder.ToTable("Servicos");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).HasColumnName("ServicoId");
        builder.Property(s => s.Name).HasColumnName("Nome").IsRequired().HasMaxLength(100);
        builder.Property(s => s.Description).HasColumnName("Descricao").HasMaxLength(500);
        builder.Property(s => s.Recruitment).HasColumnName("Recrutamento").IsRequired();
        builder.Property(s => s.Selection).HasColumnName("Selecao").IsRequired();
        builder.Property(s => s.Proposal).HasColumnName("Proposta").IsRequired();
        builder.Property(s => s.Active).HasColumnName("Ativo").IsRequired();
    }
}
