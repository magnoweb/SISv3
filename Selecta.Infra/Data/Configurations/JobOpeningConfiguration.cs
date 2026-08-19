using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Recruitment;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Mapeia a entidade JobOpening (nomes em inglês) para a tabela "Vagas" já
/// existente (colunas em português). Equivalente EF Core da antiga
/// VagaConfig (EF6) — os HasRequired(...) de navegação do original
/// (Responsavel/Contato/Cargo/EtapaRecrutamento) agora são relações reais
/// aqui também, incluindo a 2ª relação com User (CreatedBy) que o original
/// também tinha (Usuario). Ambas apontam para "Usuarios", por isso usam FKs
/// e navegações distintas para o EF Core conseguir diferenciá-las.
/// Sem os Ignore(TempoVaga/DiasTrabalhados) do original, já que esses dois
/// viraram campos calculados no DTO, não propriedades da entidade.
/// </summary>
public class JobOpeningConfiguration : IEntityTypeConfiguration<JobOpening>
{
    public void Configure(EntityTypeBuilder<JobOpening> builder)
    {
        builder.ToTable("Vagas");
        builder.HasKey(j => j.Id);

        builder.Property(j => j.Id).HasColumnName("VagaId");
        builder.Property(j => j.TicketId).HasColumnName("TicketId").IsRequired();
        builder.Property(j => j.ManagerId).HasColumnName("ResponsavelId").IsRequired();
        builder.Property(j => j.ContactId).HasColumnName("ContatoId").IsRequired();
        builder.Property(j => j.JobTitleId).HasColumnName("CargoId").IsRequired();
        builder.Property(j => j.RecruitmentStageId).HasColumnName("EtapaRecrutamentoId").IsRequired();
        builder.Property(j => j.Name).HasColumnName("Nome").IsRequired().HasMaxLength(150);
        builder.Property(j => j.Summary).HasColumnName("Resumo").HasMaxLength(1000);
        builder.Property(j => j.DeadlineDays).HasColumnName("Prazo").IsRequired();
        builder.Property(j => j.Quantity).HasColumnName("Quantidade").IsRequired();
        builder.Property(j => j.Salary).HasColumnName("Salario");
        builder.Property(j => j.Status).HasColumnName("Status").IsRequired();
        builder.Property(j => j.CreatedById).HasColumnName("UsuarioId").IsRequired();
        builder.Property(j => j.PaymentDate).HasColumnName("DataPagamento");
        builder.Property(j => j.ClosedAt).HasColumnName("DataFechamento");
        builder.Property(j => j.CreatedAt).HasColumnName("DataInclusao").IsRequired();

        builder.HasOne(j => j.Manager)
            .WithMany()
            .HasForeignKey(j => j.ManagerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(j => j.CreatedBy)
            .WithMany()
            .HasForeignKey(j => j.CreatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(j => j.Contact)
            .WithMany()
            .HasForeignKey(j => j.ContactId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(j => j.JobTitle)
            .WithMany()
            .HasForeignKey(j => j.JobTitleId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(j => j.RecruitmentStage)
            .WithMany()
            .HasForeignKey(j => j.RecruitmentStageId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
