using Acti.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acti.Infra.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd()
            .HasColumnType("int")
            .UseIdentityColumn();
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(80)
            .HasColumnName("name")
            .HasColumnType("VARCHAR(80)");

        builder.Property(t => t.Email)
            .IsRequired()
            .HasMaxLength(180)
            .HasColumnName("email")
            .HasColumnType("VARCHAR(180)");

        builder.Property(t => t.Password)
            .IsRequired()
            .HasMaxLength(280)
            .HasColumnName("password")
            .HasColumnType("VARCHAR(280)");

        builder.Property(t => t.Avatar)
            .IsRequired()
            .HasMaxLength(180)
            .HasColumnName("avatar")
            .HasColumnType("VARCHAR(180)");

        builder.Property(t => t.ResetToken)
            .IsRequired(false)
            .HasMaxLength(180)
            .HasColumnName("resetToken")
            .HasColumnType("VARCHAR(180)");
    }
}