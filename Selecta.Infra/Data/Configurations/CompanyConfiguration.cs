using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Common;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Mapeia Company para a tabela "Empresas" já existente. Equivalente EF Core
/// da antiga EmpresaConfig (EF6). A FK opcional para City (CidadeId) é uma
/// relação real — City já existe neste projeto.
/// </summary>
public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Empresas");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("EmpresaId");
        builder.Property(c => c.Type).HasColumnName("TipoEmpresa").IsRequired();
        builder.Property(c => c.LegalName).HasColumnName("RazaoSocial").IsRequired().HasMaxLength(150);
        builder.Property(c => c.TradeName).HasColumnName("NomeFantasia").IsRequired().HasMaxLength(50);
        builder.Property(c => c.Document).HasColumnName("Documento").IsRequired().HasMaxLength(20);
        builder.Property(c => c.StateRegistration).HasColumnName("InscricaoEstadual").HasMaxLength(20);
        builder.Property(c => c.Address).HasColumnName("Endereco").HasMaxLength(150);
        builder.Property(c => c.AddressComplement).HasColumnName("Complemento").HasMaxLength(50);
        builder.Property(c => c.Neighborhood).HasColumnName("Bairro").HasMaxLength(50);
        builder.Property(c => c.CityName).HasColumnName("Cidade").HasMaxLength(50);
        builder.Property(c => c.State).HasColumnName("Estado");
        builder.Property(c => c.PostalCode).HasColumnName("Cep").HasMaxLength(10);
        builder.Property(c => c.Phone1).HasColumnName("Telefone1").HasMaxLength(20);
        builder.Property(c => c.Phone2).HasColumnName("Telefone2").HasMaxLength(20);
        builder.Property(c => c.Notes).HasColumnName("Observacoes").HasMaxLength(500);
        builder.Property(c => c.CreatedAt).HasColumnName("DataInclusao").IsRequired();
        builder.Property(c => c.Active).HasColumnName("Ativo").IsRequired();
        builder.Property(c => c.CityId).HasColumnName("CidadeId");

        builder.HasIndex(c => c.Document).IsUnique();

        builder.HasOne(c => c.City)
            .WithMany()
            .HasForeignKey(c => c.CityId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
