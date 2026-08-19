using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Security;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Equivalente EF Core da antiga PerfilAcessoConfig (EF6).</summary>
public class AccessProfileConfiguration : IEntityTypeConfiguration<AccessProfile>
{
    public void Configure(EntityTypeBuilder<AccessProfile> builder)
    {
        builder.ToTable("PerfisAcesso");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("PerfilAcessoId");
        builder.Property(p => p.Name).HasColumnName("Nome").IsRequired().HasMaxLength(50);
        builder.Property(p => p.Description).HasColumnName("Descricao").HasMaxLength(500);
    }
}
