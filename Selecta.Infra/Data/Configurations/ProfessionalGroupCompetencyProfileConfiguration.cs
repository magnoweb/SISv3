using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Só a coluna/FK própria deste subtipo — TPH, sem ToTable (herdado de
/// CompetencyProfileConfiguration). "GrupoProfissionalId" é nullable na BD
/// (partilhada com o outro subtipo), por isso a FK é opcional aqui também.
/// </summary>
public class ProfessionalGroupCompetencyProfileConfiguration : IEntityTypeConfiguration<ProfessionalGroupCompetencyProfile>
{
    public void Configure(EntityTypeBuilder<ProfessionalGroupCompetencyProfile> builder)
    {
        builder.Property(p => p.ProfessionalGroupId).HasColumnName("GrupoProfissionalId");

        builder.HasOne(p => p.ProfessionalGroup)
            .WithMany()
            .HasForeignKey(p => p.ProfessionalGroupId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
