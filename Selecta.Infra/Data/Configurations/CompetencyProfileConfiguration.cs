using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Configura a hierarquia TPH (Table Per Hierarchy) da base abstrata para
/// a tabela "Perfis" já existente. Valores de discriminador confirmados
/// contra o schema real (script de criação da BD): coluna "Discriminator"
/// nvarchar(128) NOT NULL, com os nomes exatos das classes CLR originais —
/// incluindo o erro de digitação "PerfilGrupoProfisisonal" (não
/// "Profissional"), que tem de ser reproduzido tal e qual porque é
/// literalmente o que já está gravado na coluna.
/// </summary>
public class CompetencyProfileConfiguration : IEntityTypeConfiguration<CompetencyProfile>
{
    public void Configure(EntityTypeBuilder<CompetencyProfile> builder)
    {
        builder.ToTable("Perfis");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("PerfilId");
        builder.Property(p => p.Name).HasColumnName("Nome").IsRequired().HasMaxLength(150);
        builder.Property(p => p.Description).HasColumnName("Descricao").HasMaxLength(500);
        builder.Property(p => p.CreatedAt).HasColumnName("DataInclusao").IsRequired();
        builder.Property(p => p.Active).HasColumnName("Ativo").IsRequired();

        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<JobTitleCompetencyProfile>("PerfilCargo")
            .HasValue<ProfessionalGroupCompetencyProfile>("PerfilGrupoProfisisonal");
    }
}
