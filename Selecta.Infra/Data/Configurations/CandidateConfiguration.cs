using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Common;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Mapeia a entidade Candidate (nomes em inglês) para a tabela "Candidatos"
/// já existente (colunas em português). Equivalente EF Core da antiga
/// CandidatoConfig (EF6).
/// </summary>
public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToTable("Candidatos");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("CandidatoId");
        builder.Property(c => c.Name).HasColumnName("Nome").IsRequired().HasMaxLength(150);
        builder.Property(c => c.Gender).HasColumnName("Genero").IsRequired();
        builder.Property(c => c.BirthDate).HasColumnName("DataNascimento").IsRequired();
        builder.Property(c => c.Cpf).HasColumnName("CPF").IsRequired();
        builder.Property(c => c.IdentityDocument).HasColumnName("Identidade").IsRequired();
        builder.Property(c => c.CreatedAt).HasColumnName("DataInclusao").IsRequired();

        builder.HasIndex(c => c.Name);
        builder.HasIndex(c => c.Cpf).IsUnique();
    }
}
