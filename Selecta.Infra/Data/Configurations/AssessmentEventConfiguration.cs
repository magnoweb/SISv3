using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Selection;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Mapeia AssessmentEvent para a tabela "EventosAvaliacao" já existente.
/// Equivalente EF Core da antiga EventoAvaliacaoConfig (EF6). Candidate e
/// JobTitle são obrigatórios; Contact, City e EvaluationResult são
/// opcionais — todas com <c>DeleteBehavior.Restrict</c>.
/// </summary>
public class AssessmentEventConfiguration : IEntityTypeConfiguration<AssessmentEvent>
{
    public void Configure(EntityTypeBuilder<AssessmentEvent> builder)
    {
        builder.ToTable("EventosAvaliacao");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("EventoAvaliacaoId");
        builder.Property(e => e.CandidateId).HasColumnName("CandidatoId").IsRequired();
        builder.Property(e => e.JobTitleId).HasColumnName("CargoId").IsRequired();
        builder.Property(e => e.ContactId).HasColumnName("ContatoId");
        builder.Property(e => e.Date).HasColumnName("Data").IsRequired();
        builder.Property(e => e.EducationLevel).HasColumnName("Escolaridade").IsRequired();
        builder.Property(e => e.Education).HasColumnName("Formacao");
        builder.Property(e => e.EducationCompleted).HasColumnName("FormacaoConcluida").IsRequired();
        builder.Property(e => e.MaritalStatus).HasColumnName("EstadoCivil").IsRequired();
        builder.Property(e => e.DriverLicenseNumber).HasColumnName("NumeroHabilitacao").HasMaxLength(20);
        builder.Property(e => e.DriverLicenseCategory).HasColumnName("CategoriaHabilitacao").IsRequired();
        builder.Property(e => e.NumberOfChildren).HasColumnName("NumeroFilhos");
        builder.Property(e => e.Address).HasColumnName("Endereco").HasMaxLength(150);
        builder.Property(e => e.AddressComplement).HasColumnName("Complemento").HasMaxLength(50);
        builder.Property(e => e.Neighborhood).HasColumnName("Bairro").HasMaxLength(50);
        builder.Property(e => e.CityName).HasColumnName("Cidade").HasMaxLength(50);
        builder.Property(e => e.State).HasColumnName("Estado");
        builder.Property(e => e.PostalCode).HasColumnName("Cep").HasMaxLength(10);
        builder.Property(e => e.Phone1).HasColumnName("Telefone1").HasMaxLength(20);
        builder.Property(e => e.Phone2).HasColumnName("Telefone2").HasMaxLength(20);
        builder.Property(e => e.Email).HasColumnName("Email").HasMaxLength(150);
        builder.Property(e => e.CityId).HasColumnName("CidadeId");
        builder.Property(e => e.Result).HasColumnName("Resultado").IsRequired();
        builder.Property(e => e.EvaluationResultId).HasColumnName("AvaliacaoResultadoId");
        builder.Property(e => e.Status).HasColumnName("Status").IsRequired();
        builder.Property(e => e.Purpose).HasColumnName("Finalidade").IsRequired();
        builder.Property(e => e.CreatedAt).HasColumnName("DataInclusao").IsRequired();
        builder.Property(e => e.CompletedAt).HasColumnName("DataConclusao");

        builder.HasOne(e => e.Candidate)
            .WithMany()
            .HasForeignKey(e => e.CandidateId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(e => e.JobTitle)
            .WithMany()
            .HasForeignKey(e => e.JobTitleId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(e => e.Contact)
            .WithMany()
            .HasForeignKey(e => e.ContactId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(e => e.City)
            .WithMany()
            .HasForeignKey(e => e.CityId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(e => e.EvaluationResult)
            .WithMany()
            .HasForeignKey(e => e.EvaluationResultId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
