using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga TestePsicologicoConfig (EF6).</summary>
public class PsychologicalTestConfiguration : IEntityTypeConfiguration<PsychologicalTest>
{
    public void Configure(EntityTypeBuilder<PsychologicalTest> builder)
    {
        builder.ToTable("TestesPsicologico");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id).HasColumnName("TestePsicologicoId");
        builder.Property(t => t.Name).HasColumnName("Nome").IsRequired().HasMaxLength(100);
        builder.Property(t => t.Description).HasColumnName("Descricao").HasMaxLength(500);
        builder.Property(t => t.CreatedAt).HasColumnName("DataInclusao").IsRequired();
        builder.Property(t => t.Active).HasColumnName("Ativo").IsRequired();
    }
}
