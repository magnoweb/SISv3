using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Mapeia OpinionList para a tabela "ListasParecer" já existente. Duas FKs
/// para "Usuarios" (Responsible/CreatedBy) com significados diferentes —
/// mesmo critério de FKs distintas para a mesma tabela já usado em
/// JobOpening/Report.
/// </summary>
public class OpinionListConfiguration : IEntityTypeConfiguration<OpinionList>
{
    public void Configure(EntityTypeBuilder<OpinionList> builder)
    {
        builder.ToTable("ListasParecer");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id).HasColumnName("ListaParecerId");
        builder.Property(l => l.ContactId).HasColumnName("ContatoId").IsRequired();
        builder.Property(l => l.ResponsibleId).HasColumnName("ResponsavelId").IsRequired();
        builder.Property(l => l.Code).HasColumnName("Nome").IsRequired().HasMaxLength(20);
        builder.Property(l => l.Date).HasColumnName("Data").IsRequired();
        builder.Property(l => l.Notes).HasColumnName("Observacoes").HasMaxLength(500);
        builder.Property(l => l.CreatedById).HasColumnName("UsuarioId").IsRequired();
        builder.Property(l => l.CreatedAt).HasColumnName("DataInclusao").IsRequired();

        builder.HasOne(l => l.Contact)
            .WithMany()
            .HasForeignKey(l => l.ContactId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(l => l.Responsible)
            .WithMany()
            .HasForeignKey(l => l.ResponsibleId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(l => l.CreatedBy)
            .WithMany()
            .HasForeignKey(l => l.CreatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
