using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Administrative;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Equivalente EF Core da antiga PropostaConfig (EF6). "Dias" (TempoVaga-like
/// calculado) não existe mais na entidade — ver ProposalService.CalculateDays.
/// </summary>
public class ProposalConfiguration : IEntityTypeConfiguration<Proposal>
{
    public void Configure(EntityTypeBuilder<Proposal> builder)
    {
        builder.ToTable("Propostas");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("PropostaId");
        builder.Property(p => p.ServiceOfferingId).HasColumnName("ServicoId").IsRequired();
        builder.Property(p => p.ProspectCompanyId).HasColumnName("EmpresaTempId").IsRequired();
        builder.Property(p => p.CreatedById).HasColumnName("UsuarioId").IsRequired();
        builder.Property(p => p.Name).HasColumnName("Nome").IsRequired().HasMaxLength(150);
        builder.Property(p => p.Description).HasColumnName("Descricao");
        builder.Property(p => p.Status).HasColumnName("Status").IsRequired();
        builder.Property(p => p.DeclineReason).HasColumnName("MotivoRecusa");
        builder.Property(p => p.CreatedAt).HasColumnName("DataInclusao").IsRequired();
        builder.Property(p => p.UpdatedAt).HasColumnName("DataAtualizacao");

        builder.HasOne(p => p.ServiceOffering)
            .WithMany()
            .HasForeignKey(p => p.ServiceOfferingId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(p => p.ProspectCompany)
            .WithMany()
            .HasForeignKey(p => p.ProspectCompanyId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(p => p.CreatedBy)
            .WithMany()
            .HasForeignKey(p => p.CreatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
