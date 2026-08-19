using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Recruitment;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Mapeia RecruitmentStage para a tabela "EtapasRecrutamento" já existente.
/// Equivalente EF Core da antiga EtapaRecrutamentoConfig (EF6).
/// </summary>
public class RecruitmentStageConfiguration : IEntityTypeConfiguration<RecruitmentStage>
{
    public void Configure(EntityTypeBuilder<RecruitmentStage> builder)
    {
        builder.ToTable("EtapasRecrutamento");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).HasColumnName("EtapaRecrutamentoId");
        builder.Property(s => s.Name).HasColumnName("Nome").IsRequired().HasMaxLength(150);
        builder.Property(s => s.Description).HasColumnName("Descricao").HasMaxLength(500);
        builder.Property(s => s.CssClass).HasColumnName("EtapaCss").HasMaxLength(50);
        builder.Property(s => s.Order).HasColumnName("Ordem").IsRequired();
        builder.Property(s => s.Active).HasColumnName("Ativo").IsRequired();
        builder.Property(s => s.CreatedAt).HasColumnName("DataInclusao").IsRequired();
    }
}
