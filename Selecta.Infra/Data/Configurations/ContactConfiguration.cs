using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Common;

namespace Selecta.Infra.Data.Configurations;

/// <summary>Mapeia Contact para a tabela "Contatos" já existente. Equivalente EF Core da antiga ContatoConfig (EF6).</summary>
public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contatos");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("ContatoId");
        builder.Property(c => c.CompanyId).HasColumnName("EmpresaId").IsRequired();
        builder.Property(c => c.Name).HasColumnName("Nome").IsRequired().HasMaxLength(150);
        builder.Property(c => c.Gender).HasColumnName("Genero").IsRequired();
        builder.Property(c => c.Position).HasColumnName("Cargo").HasMaxLength(50);
        builder.Property(c => c.Phone1).HasColumnName("Telefone1").HasMaxLength(20);
        builder.Property(c => c.Phone2).HasColumnName("Telefone2").HasMaxLength(20);
        builder.Property(c => c.Email).HasColumnName("Email").IsRequired();
        builder.Property(c => c.BirthDay).HasColumnName("DiaAniversario");
        builder.Property(c => c.BirthMonth).HasColumnName("MesAniversario");
        builder.Property(c => c.Notes).HasColumnName("Observacoes").HasMaxLength(500);
        builder.Property(c => c.ReceiveNotifications).HasColumnName("ReceberNotificacoes").IsRequired();
        builder.Property(c => c.Active).HasColumnName("Ativo").IsRequired();
        builder.Property(c => c.CreatedAt).HasColumnName("DataInclusao").IsRequired();

        builder.HasOne(c => c.Company)
            .WithMany()
            .HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
