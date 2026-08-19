using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Administrative;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga EmpresaTempConfig (EF6).</summary>
public class ProspectCompanyConfiguration : IEntityTypeConfiguration<ProspectCompany>
{
    public void Configure(EntityTypeBuilder<ProspectCompany> builder)
    {
        builder.ToTable("EmpresasTemp");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("EmpresaTempId");
        builder.Property(p => p.CompanyId).HasColumnName("EmpresaId");
        builder.Property(p => p.Name).HasColumnName("Nome").IsRequired().HasMaxLength(150);
        builder.Property(p => p.Document).HasColumnName("Documento").IsRequired().HasMaxLength(20);

        builder.HasIndex(p => p.Document).IsUnique();

        builder.HasOne(p => p.Company)
            .WithMany()
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
