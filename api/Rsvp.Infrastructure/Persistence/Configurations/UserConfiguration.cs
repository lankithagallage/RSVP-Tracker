namespace Rsvp.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Rsvp.Domain.Contexts.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.HasKey(u => u.Id);

    builder.Property(u => u.FirstName)
      .IsRequired()
      .HasColumnType("varchar(200)")
      .HasMaxLength(200);

    builder.Property(u => u.LastName)
      .IsRequired()
      .HasColumnType("varchar(200)")
      .HasMaxLength(200);

    builder.Ignore(u => u.FullName);

    builder.Property(u => u.Email)
      .IsRequired()
      .HasColumnType("varchar(200)")
      .HasMaxLength(200);

    builder.HasIndex(u => u.Email)
      .IsUnique();

    builder.Property(u => u.Role)
      .IsRequired()
      .HasColumnType("varchar(20)")
      .HasConversion<string>();
  }
}
