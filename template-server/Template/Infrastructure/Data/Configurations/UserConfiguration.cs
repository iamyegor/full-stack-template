using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users").HasKey(user => user.Id);

        builder.Property(user => user.Id).HasColumnName("id").ValueGeneratedNever();
        builder.ComplexProperty(
            user => user.Email,
            eBuilder =>
            {
                eBuilder.Property(x => x.Value).HasColumnName("email");
            }
        );
    }
}
