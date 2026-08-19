using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selecta.Core.Entities.Security;

namespace Selecta.Infra.Data.Configurations;

/// <summary>
/// Mapeia a entidade User (nomes em inglês) para a tabela "Usuarios" já
/// existente (colunas em português). Equivalente EF Core da antiga
/// UsuarioConfig (EF6). Mapeia apenas as colunas que existem em
/// <see cref="User"/> — a tabela original tem mais colunas (EmpresaId,
/// ContatoId, ColaboradorId, Foto, Token, ...) que continuam na base de
/// dados mas não são tocadas por este projeto ainda.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("UsuarioId");
        builder.Property(u => u.Name).HasColumnName("Nome").IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).HasColumnName("Email").IsRequired().HasMaxLength(150);
        builder.Property(u => u.Login).HasColumnName("Login").IsRequired().HasMaxLength(20);
        builder.Property(u => u.PasswordHash).HasColumnName("Senha").IsRequired().HasMaxLength(50);
        builder.Property(u => u.Roles).HasColumnName("Perfis").HasMaxLength(500);
        builder.Property(u => u.Active).HasColumnName("Ativo").IsRequired();
        builder.Property(u => u.IsSystemAdmin).HasColumnName("SysAdmin").IsRequired();
        builder.Property(u => u.CreatedAt).HasColumnName("DataInclusao").IsRequired();

        builder.HasIndex(u => u.Login).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
    }
}
