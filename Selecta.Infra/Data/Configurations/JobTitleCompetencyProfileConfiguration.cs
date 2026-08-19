using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Só a coluna/FK própria deste subtipo — TPH, sem ToTable (herdado de
/// CompetencyProfileConfiguration). "CargoId" é nullable na BD (partilhada
/// com o outro subtipo), por isso a FK é opcional aqui também.
/// </summary>
public class JobTitleCompetencyProfileConfiguration : IEntityTypeConfiguration<JobTitleCompetencyProfile>
{
    public void Configure(EntityTypeBuilder<JobTitleCompetencyProfile> builder)
    {
        builder.Property(p => p.JobTitleId).HasColumnName("CargoId");

        builder.HasOne(p => p.JobTitle)
            .WithMany()
            .HasForeignKey(p => p.JobTitleId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
