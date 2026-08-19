using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Common;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Mapeia JobTitle para a tabela "Cargos" já existente. Equivalente EF Core da antiga CargoConfig (EF6).</summary>
public class JobTitleConfiguration : IEntityTypeConfiguration<JobTitle>
{
    public void Configure(EntityTypeBuilder<JobTitle> builder)
    {
        builder.ToTable("Cargos");
        builder.HasKey(j => j.Id);

        builder.Property(j => j.Id).HasColumnName("CargoId");
        builder.Property(j => j.CompanyId).HasColumnName("EmpresaId").IsRequired();
        builder.Property(j => j.ProfessionalGroupId).HasColumnName("GrupoProfissionalId").IsRequired();
        builder.Property(j => j.Name).HasColumnName("Nome").IsRequired().HasMaxLength(150);
        builder.Property(j => j.Description).HasColumnName("Descricao").HasMaxLength(500);
        builder.Property(j => j.CreatedAt).HasColumnName("DataInclusao").IsRequired();
        builder.Property(j => j.Active).HasColumnName("Ativo").IsRequired();

        builder.HasOne(j => j.Company)
            .WithMany()
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(j => j.ProfessionalGroup)
            .WithMany()
            .HasForeignKey(j => j.ProfessionalGroupId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
