using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo.Domain.Entities;

namespace Modelo.Infra.Data.Mapping
{
    public class UserMap : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("User");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Cpf)
                .IsRequired()
                .HasColumnName("Cpf");

            builder.Property(c => c.BirthDate)
                .IsRequired()
                .HasColumnName("BirthDate");

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnName("Name");

            builder.Property(c => c.Email)
                .IsRequired()
                .HasColumnName("Email");

            builder.Property(c => c.Password)
              .IsRequired()
              .HasColumnName("Password");
        }
    }
}
